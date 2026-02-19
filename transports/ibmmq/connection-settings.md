---
title: Connection Settings
summary: Connection settings for the IBM MQ transport, including SSL/TLS, high availability, and advanced configuration
reviewed: 2026-02-19
component: IbmMq
---

## Basic connection

The transport connects to an IBM MQ queue manager using a host, port, and SVRCONN channel:

```csharp
var transport = new IbmMqTransport(options =>
{
    options.Host = "mq-server.example.com";
    options.Port = 1414;
    options.Channel = "DEV.APP.SVRCONN";
    options.QueueManagerName = "QM1";
});
```

### Defaults

|Setting|Default|
|:---|---|
|Host|`localhost`|
|Port|`1414`|
|Channel|`DEV.ADMIN.SVRCONN`|
|QueueManagerName|Empty (local default queue manager)|

## Authentication

User credentials can be provided to authenticate with the queue manager:

```csharp
var transport = new IbmMqTransport(options =>
{
    options.Host = "mq-server.example.com";
    options.QueueManagerName = "QM1";
    options.User = "app";
    options.Password = "passw0rd";
});
```

> [!NOTE]
> When no credentials are provided, the connection uses the operating system identity. This may be appropriate for local development but typically requires explicit credentials in production.

## Application name

The application name appears in IBM MQ monitoring tools and is useful for identifying connections:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.ApplicationName = "OrderService";
});
```

If not specified, the application name defaults to the entry assembly name.

## High availability

For high availability scenarios with multi-instance queue managers, provide a connection name list instead of a single host and port:

```csharp
var transport = new IbmMqTransport(options =>
{
    options.QueueManagerName = "QM1";
    options.Channel = "APP.SVRCONN";
    options.Connections.Add("mqhost1(1414)");
    options.Connections.Add("mqhost2(1414)");
});
```

When `Connections` is specified, the `Host` and `Port` properties are ignored. The client will attempt each connection in order, connecting to the first available queue manager.

> [!NOTE]
> All entries in the connection name list must point to instances of the same queue manager (by name). This provides failover to a standby instance, not load balancing.

## SSL/TLS

To enable encrypted communication, configure the SSL key repository and cipher specification. The cipher must match the `SSLCIPH` attribute on the SVRCONN channel.

```csharp
var transport = new IbmMqTransport(options =>
{
    options.Host = "mq-server.example.com";
    options.QueueManagerName = "QM1";
    options.SslKeyRepository = "*SYSTEM";
    options.CipherSpec = "TLS_RSA_WITH_AES_256_CBC_SHA256";
});
```

### Key repository options

|Value|Description|
|:---|---|
|`*SYSTEM`|Windows system certificate store|
|`*USER`|Windows user certificate store|
|File path|Path to a key database file (without extension), e.g., `/var/mqm/ssl/key`|

### Peer name verification

Verify the queue manager's certificate distinguished name for additional security:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.SslKeyRepository = "*SYSTEM";
    options.CipherSpec = "TLS_RSA_WITH_AES_256_CBC_SHA256";
    options.SslPeerName = "CN=MQSERVER01,O=MyCompany,C=US";
});
```

> [!NOTE]
> Both `SslKeyRepository` and `CipherSpec` must be specified together. Setting one without the other will cause a configuration validation error.

## Topic naming

Topics are named using a configurable `TopicNaming` strategy. The default uses a prefix of `DEV` and the fully qualified type name.

### Custom topic prefix

To change the prefix:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.TopicNaming = new TopicNaming("PROD");
});
```

### Custom topic naming strategy

IBM MQ topic names are limited to 48 characters. If event type names are long, subclass `TopicNaming` to implement a shortening strategy:

```csharp
public class ShortTopicNaming() : TopicNaming("APP")
{
    public override string GenerateTopicName(Type eventType)
    {
        return $"APP.{eventType.Name}".ToUpperInvariant();
    }
}
```

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.TopicNaming = new ShortTopicNaming();
});
```

## Resource name sanitization

IBM MQ queue and topic names are limited to 48 characters and allow only `A-Z`, `a-z`, `0-9`, `.`, and `_`. If endpoint names contain invalid characters or are too long, configure a sanitizer:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.ResourceNameSanitizer = name =>
    {
        var sanitized = name.Replace("-", ".").Replace("/", ".");
        return sanitized.Length > 48 ? sanitized[..48] : sanitized;
    };
});
```

> [!WARNING]
> Ensure the sanitizer produces deterministic and unique names. Two different input names mapping to the same sanitized name will cause messages to be delivered to the wrong endpoint.

## Message processing settings

### Polling interval

The wait interval controls how long each poll waits for a message before returning:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.MessageWaitInterval = TimeSpan.FromMilliseconds(2000);
});
```

|Setting|Default|Range|
|:---|---|---|
|MessageWaitInterval|5000 ms|100–30,000 ms|

### Maximum message size

Should match or be less than the queue manager's `MAXMSGL` setting:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.MaxMessageLength = 10 * 1024 * 1024; // 10 MB
});
```

|Setting|Default|Range|
|:---|---|---|
|MaxMessageLength|4 MB|1 KB – 100 MB|

### Character set

The Coded Character Set Identifier (CCSID) used for message text encoding. The default is UTF-8 (1208), which is recommended for most scenarios.

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.CharacterSet = 1208; // UTF-8 (default)
});
```

## Message persistence

By default, all messages are sent as persistent, meaning they survive queue manager restarts. Messages marked with the `NonDurableMessage` header are sent as non-persistent for higher throughput.

> [!CAUTION]
> Non-persistent messages are lost if the queue manager restarts before they are consumed.
