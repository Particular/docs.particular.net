---
title: Connection Settings
summary: Connection settings for the IBM MQ transport, including SSL/TLS, high availability, and advanced configuration
reviewed: 2026-02-19
component: IbmMq
---

## Basic connection

The transport connects to an IBM MQ queue manager using a host, port, and SVRCONN channel:

snippet: ibmmq-basic-connection

### Defaults

|Setting|Default|
|:---|---|
|Host|`localhost`|
|Port|`1414`|
|Channel|`DEV.ADMIN.SVRCONN`|
|QueueManagerName|Empty (local default queue manager)|

## Authentication

User credentials can be provided to authenticate with the queue manager:

snippet: ibmmq-authentication

> [!NOTE]
> When no credentials are provided, the connection uses the operating system identity. This may be appropriate for local development but typically requires explicit credentials in production.

## Application name

The application name appears in IBM MQ monitoring tools and is useful for identifying connections:

snippet: ibmmq-application-name

If not specified, the application name defaults to the entry assembly name.

## High availability

For high availability scenarios with multi-instance queue managers, provide a connection name list instead of a single host and port:

snippet: ibmmq-high-availability

When `Connections` is specified, the `Host` and `Port` properties are ignored. The client will attempt each connection in order, connecting to the first available queue manager.

> [!NOTE]
> All entries in the connection name list must point to instances of the same queue manager (by name). This provides failover to a standby instance, not load balancing.

## SSL/TLS

To enable encrypted communication, configure the SSL key repository and cipher specification. The cipher must match the `SSLCIPH` attribute on the SVRCONN channel.

snippet: ibmmq-ssl-tls

### Key repository options

|Value|Description|
|:---|---|
|`*SYSTEM`|Windows system certificate store|
|`*USER`|Windows user certificate store|
|File path|Path to a key database file (without extension), e.g., `/var/mqm/ssl/key`|

### Peer name verification

Verify the queue manager's certificate distinguished name for additional security:

snippet: ibmmq-ssl-peer-name

> [!NOTE]
> Both `SslKeyRepository` and `CipherSpec` must be specified together. Setting one without the other will cause a configuration validation error.

## Topic naming

Topics are named using a configurable `TopicNaming` strategy. The default uses a prefix of `DEV` and the fully qualified type name.

### Custom topic prefix

To change the prefix:

snippet: ibmmq-custom-topic-prefix

### Custom topic naming strategy

IBM MQ topic names are limited to 48 characters. If event type names are long, subclass `TopicNaming` to implement a shortening strategy:

snippet: ibmmq-custom-topic-naming-class

snippet: ibmmq-custom-topic-naming-usage

## Resource name sanitization

IBM MQ queue and topic names are limited to 48 characters and allow only `A-Z`, `a-z`, `0-9`, `.`, and `_`. If endpoint names contain invalid characters or are too long, configure a sanitizer:

snippet: ibmmq-resource-sanitization

> [!WARNING]
> Ensure the sanitizer produces deterministic and unique names. Two different input names mapping to the same sanitized name will cause messages to be delivered to the wrong endpoint.

## Message processing settings

### Polling interval

The wait interval controls how long each poll waits for a message before returning:

snippet: ibmmq-polling-interval

|Setting|Default|Range|
|:---|---|---|
|MessageWaitInterval|5000 ms|100–30,000 ms|

### Maximum message size

Should match or be less than the queue manager's `MAXMSGL` setting:

snippet: ibmmq-max-message-size

|Setting|Default|Range|
|:---|---|---|
|MaxMessageLength|4 MB|1 KB – 100 MB|

### Character set

The Coded Character Set Identifier (CCSID) used for message text encoding. The default is UTF-8 (1208), which is recommended for most scenarios.

snippet: ibmmq-character-set

## Message persistence

By default, all messages are sent as persistent, meaning they survive queue manager restarts. Messages marked with the `NonDurableMessage` header are sent as non-persistent for higher throughput.

> [!CAUTION]
> Non-persistent messages are lost if the queue manager restarts before they are consumed.
