---
title: Azure Storage Queues Transport Upgrade Version 9 to 10
summary: Instructions on how to upgrade the Azure Storage Queues transport from version 9 to 10.
reviewed: 2021-03-12
component: ASQ
related:
- transports/azure-storage-queues
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Cross Account Routing

### Sending a message

To send a message to a receiver on another storage account, the configuration

```csharp
var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
var routing = transportConfig
                    .ConnectionString("connectionString")
                    .AccountRouting();
var anotherAccount = routing.AddAccount("AnotherAccountName","anotherConnectionString");
anotherAccount.RegisteredEndpoints.Add("Receiver");

transportConfig.Routing().RouteToEndpoint(typeof(MyMessage), "Receiver");

```

becomes:

```csharp
var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();

var routing = transportConfig
                    .ConnectionString("connectionString")
                    .AccountRouting();
var anotherAccount = routing.AddAccount("AnotherAccountName","anotherConnectionString");
anotherAccount.AddEndpoint("Receiver");

transportConfig.Routing().RouteToEndpoint(typeof(MyMessage), "Receiver");
```

### Subscribing on an event

To subscribe to an event, coming from a publisher on another storage account, the configuration

```csharp
var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
var routing = transportConfig
                    .ConnectionString("anotherConnectionString")
                    .AccountRouting();
var anotherAccount = routing.AddAccount("PublisherAccountName", "connectionString");
anotherAccount.AddEndpoint("Publisher", new[] { typeof(MyEvent) });
anotherAccount.RegisteredEndpoints.Add("Publisher");

transportConfig.Routing().RegisterPublisher(typeof(MyEvent), "Publisher");
```

becomes:

```csharp
 var transportConfig = configuration.UseTransport<AzureStorageQueueTransport>();
var routing = transportConfig
                    .ConnectionString("anotherConnectionString")
                    .AccountRouting();
var anotherAccount = routing.AddAccount("PublisherAccountName", "connectionString");
anotherAccount.AddEndpoint("Publisher", new[] { typeof(MyEvent) });
```