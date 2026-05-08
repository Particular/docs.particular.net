---
title: Azure SQL failover and connection pooling
summary: How to handle stale connections after Azure SQL failover events when using the SQL Server transport
component: SqlTransport
versions: '[8,)'
reviewed: 2026-03-19
related:
 - transports/sql/connection-settings
 - transports/sql/sql-azure
---

Azure SQL periodically moves databases between nodes for load balancing, patching, and other maintenance operations. These failover events are typically brief (one to two seconds of write unavailability), but the SQL Server transport may log errors for several minutes after the failover completes.

This article explains why that happens and how to reduce the error window using a [custom connection factory](/transports/sql/connection-settings.md#custom-sql-server-transport-connection-factory).

## Why failovers cause extended error windows

ADO.NET maintains a [connection pool](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-connection-pooling) of open connections for each unique connection string. When Azure SQL moves a database to a new node, the following sequence occurs:

1. Existing TCP connections to the old node become invalid.
2. ADO.NET's connection pool does not detect this immediately. From the pool's perspective, those connections were returned cleanly and appear valid.
3. When the transport's queue polling loop or a message handler requests a connection, the pool hands out a stale one.
4. The attempt to use the stale connection against the new node fails. With Microsoft Entra ID (Managed Identity) authentication, the failure is typically a "Login failed" error (SQL error 18456) rather than a network-level error, because the TCP handshake to the new node succeeds but the authentication context from the old connection is not accepted.
5. ADO.NET eventually retires stale connections on a background timer, but this can take minutes. Until then, every connection drawn from the pool may be stale.

The result is that a failover lasting under two seconds can produce three to five minutes of login errors in the application logs.

## Clearing the connection pool on transient errors

The SQL Server transport supports a [custom connection factory](/transports/sql/connection-settings.md#custom-sql-server-transport-connection-factory) that controls how connections are created and opened. By catching transient Azure SQL errors in this factory and clearing the connection pool before rethrowing, subsequent retry attempts will establish fresh connections to the new node.

snippet: sqlserver-azure-failover-connection-factory

After `ClearAllPools()` is called, the next connection attempt acquires a new authentication token and establishes a fresh TCP connection to the new node. This reduces the error window from minutes to one or two retry cycles.

### Identifying transient Azure SQL errors

The connection factory above uses a helper method to identify transient errors that indicate a failover or reconfiguration event:

snippet: sqlserver-azure-failover-transient-errors

The error numbers cover the most common Azure SQL transient failures:

| Error number | Description |
|:---|:---|
| 18456 | Login failed (common with Managed Identity after failover) |
| 233 | Connection closed by the remote host |
| 64 | Named pipe connection failed |
| 4060 | Cannot open database |
| 40197 | Error processing request during reconfiguration |
| 40613 | Database not currently available |
| 40143 | Connection could not be initialized |
| 40540 | Service goal prevented processing the request |

> [!NOTE]
> The exact error numbers observed depend on the type of failover, the authentication method, and the Azure SQL service tier. Review application logs to identify additional error numbers that may be relevant to a specific environment.

## ClearAllPools vs. ClearPool

`SqlConnection.ClearAllPools()` clears every connection pool in the process, regardless of connection string. This is simple and effective, but if the process also maintains connections to other databases (for example, an application database alongside the transport database), those pools are cleared too, causing one round of fresh connections for each.

For finer-grained control, `SqlConnection.ClearPool(connection)` targets only the pool associated with the specific connection string:

snippet: sqlserver-azure-failover-single-pool

In a typical NServiceBus endpoint where the transport is the only SQL connection, `ClearAllPools()` is the simpler choice. Use `ClearPool` when the process manages connections to multiple databases and clearing all pools would cause unnecessary overhead.

## How this interacts with NServiceBus retry behavior

The connection factory is invoked for all transport operations, including the queue polling loop and message processing. When the factory throws on a transient error:

- **Queue polling:** The transport logs the error and retries on the next poll cycle. The [queue peeker interval](/transports/sql/design.md) controls how frequently this occurs (default: one second).
- **Message processing:** The failure triggers NServiceBus [immediate retries](/nservicebus/recoverability/#immediate-retries). Because the connection pool has already been cleared, the retry will use a fresh connection.

The [circuit breaker](/transports/sql/connection-settings.md#circuit-breaker) also applies. If the database remains unavailable beyond the configured wait time (default: 30 seconds), the circuit breaker triggers [critical error](/nservicebus/hosting/critical-errors.md) handling.
