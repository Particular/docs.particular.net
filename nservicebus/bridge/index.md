---
title: Bridge
summary: Connect endpoints in a system that use different transports with the transport bridge
reviewed: 2022-04-01
component: Bridge
related:
 - samples/bridge/simple
 - samples/bridge/azure-service-bus-msmq-bridge
 - samples/bridge/sql-multi-instance
---

The `NServiceBus.Transport.Bridge` allows NServiceBus endpoints to connect to other endpoints that are not using the same transport.

The bridge is transparent to sending and receiving endpoints. That is, endpoints are not aware of the bridge or that it is transferring messages to a different transport. Endpoints send and receive messages to and from logical endpoints as they normally would if there were no bridge involved.

## Why use the bridge

The transport bridge enables several scenarios:

- Migrating from one transport to another. E.g. migration from MSMQ to a cloud-native transport. The bridge allows endpoints to be migrated one at a time rather than all at once.
- Using multiple transports because of pricing considerations. Instead of relying only on a cloud-native transport (which may be more expensive), less mission critical endpoints could operating on a cheaper transport.
- Using a transport that best fits non-functional considerations. E.g. A transport that allows bridging geographical locations, while some endpoints on a more secure transport only process private information.

More details on these scenarios is provided in the [transport bridge scenarios](scenarios.md) article.

## Bridge configuration

`NServiceBus.Transport.Bridge` is packaged as a host-agnostic library. It can be hosted in a console application, a Windows service, a Docker container, or any service that supports the Microsoft Generic Host similar to how [endpoints are hosted](/nservicebus/hosting/selecting.md).

The following snippet shows a simple MSMQ-to-AzureServiceBus configuration.

snippet: bridgeconfiguration

The life cycle of the bridge is managed by the .NET Generic Host.

## Consistency

If the bridge moves messages across different transport types or different brokers of the same type (e.g. Azure ServiceBus namespaces, or RabbitMQ vhosts), [`ReceiveOnly`](/transports/transactions.md#transactions-transport-transaction-receive-only) is the only supported transaction mode. In this mode, messages that are moved across the bridge may be duplicated if some infrastructure-related issue prevents the message from being moved to the target transport. To address this either [ensure that handlers are idempotent or enable deduplication of messages using the outbox](/transports/transactions.md#transactions-transport-transaction-receive-only-consistency-guarantees).

The bridge will use this mode if at least one of the transports being configured is not able to use distributed transactions.

### Distributed transactions

If all configured transports support the [`TransactionScope`](/transports/transactions.md#transactions-transaction-scope-distributed-transaction) transaction mode, the bridge will use the same transaction mode so that no messages are duplicated during message transfer between transports. This enables migration from the MSMQ transport to the SQL Server transport without having to make changes to the endpoints.

If `ReceiveOnly` transaction mode is preferred, use the following configuration:

snippet: bridge-configuration-explicit-receive-only-mode
