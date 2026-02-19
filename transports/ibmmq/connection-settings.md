---
title: Connection Settings
summary: Information about connection settings for the IBM MQ transport, including SSL/TLS and high availability
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

The application name appears in IBM MQ monitoring tools (e.g., `DISPLAY CONN(*)` in runmqsc) and is useful for identifying connections:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.ApplicationName = "OrderService";
});
```

If not specified, the application name defaults to the entry assembly name.

## High availability

For high availability scenarios with multi-instance queue managers or a set of candidate queue managers, provide a connection name list instead of a single host and port:

```csharp
var transport = new IbmMqTransport(options =>
{
    options.QueueManagerName = "QM1";
    options.Channel = "APP.SVRCONN";
    options.Connections.Add("mqhost1(1414)");
    options.Connections.Add("mqhost2(1414)");
});
```

When `Connections` is specified, the `Host` and `Port` properties are ignored. The IBM MQ client will attempt each connection in order, connecting to the first available queue manager.

> [!NOTE]
> All entries in the connection name list must point to instances of the same queue manager (by name). This is not a load balancing mechanism; it provides failover to a standby instance.

## SSL/TLS

To enable encrypted communication with the queue manager, configure the SSL key repository and cipher specification. The cipher must match the `SSLCIPH` attribute on the SVRCONN channel.

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
|`*SYSTEM`|Use the Windows system certificate store|
|`*USER`|Use the Windows user certificate store|
|File path|Path to a key database file (without extension), e.g., `/var/mqm/ssl/key`|

### Peer name verification

For additional security, verify the queue manager's certificate distinguished name:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.SslKeyRepository = "*SYSTEM";
    options.CipherSpec = "TLS_RSA_WITH_AES_256_CBC_SHA256";
    options.SslPeerName = "CN=MQSERVER01,O=MyCompany,C=US";
});
```

### Key reset count

SSL key renegotiation can be configured to periodically reset the secret key after a specified number of bytes:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection and SSL settings ...
    options.KeyResetCount = 40000; // Renegotiate every 40 KB
});
```

The default value of `0` disables client-side key reset, deferring to the channel or queue manager setting.

> [!NOTE]
> Both `SslKeyRepository` and `CipherSpec` must be specified together. Setting one without the other will cause a configuration validation error.

## Message processing settings

### Polling interval

The transport polls queues for messages using the IBM MQ `MQGET` with wait. The wait interval controls how long each poll waits for a message before returning:

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

Shorter intervals improve responsiveness but increase CPU usage. Longer intervals reduce CPU usage but increase message processing latency.

### Maximum message size

The maximum message size the transport will accept. This should match or be less than the queue manager's `MAXMSGL` setting:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.MaxMessageLength = 10 * 1024 * 1024; // 10 MB
});
```

|Setting|Default|Range|
|:---|---|---|
|MaxMessageLength|4 MB (4,194,304 bytes)|1 KB – 100 MB|

### Character set

The Coded Character Set Identifier (CCSID) used for message text encoding:

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
    options.CharacterSet = 1208; // UTF-8 (default)
});
```

Common values:

|CCSID|Encoding|
|:---|---|
|1208|UTF-8 (recommended)|
|819|ISO 8859-1 (Latin-1)|
|1252|Windows Latin-1|
