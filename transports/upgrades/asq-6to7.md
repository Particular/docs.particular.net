---
title: Azure Storage Queues transport upgrade version 6 to 7
summary: Instructions on how to upgrade the Azure Storage Queues transport from version 6 to version 7
reviewed: 2022-06-01
component: ASQ
related:
- transports/azure-storage-queues
- nservicebus/upgrades/5to6
- transports/azure-storage-queues/configuration
redirects:
 - nservicebus/upgrades/asq-6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---

## New Configuration API

In versions 6 and below, the Azure Storage Queues transport was configured using an XML configuration section called `AzureStorageQueueTransportConfiguration`. This section has been removed in favor of a more granular, code-based configuration API.

The new configuration API is accessible through extension methods on the `UseTransport<AzureStorageQueueTransport>()` extension point in the endpoint configuration. See also: [Azure Storage Queues Configuration](/transports/azure-storage-queues/configuration.md).

```csharp
var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
// Configure the transport
transport.ConnectionString("The Connection String");
```

### To continue reading from app.config

Add the following to the project's app.config:

```xml
<configuration>
  <appSettings>
    <add key="AzureStorageQueueConnection"
         value="The Connection String" />
  </appSettings>
</configuration>
```

Then read from app.config and pass the value to the transport configuration:

```csharp
var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
var connection = ConfigurationManager.AppSettings["AzureStorageQueueConnection"];
transport.ConnectionString(connection);
```

### Setting the configuration values via API

Setting the configuration values can now be done through the API as follows:

* [ConnectionString](/transports/azure-storage-queues/configuration.md#configuration-parameters-connectionstring)
* [BatchSize](/transports/azure-storage-queues/configuration.md#configuration-parameters-batchsize)
* [MaximumWaitTimeWhenIdle](/transports/azure-storage-queues/configuration.md#configuration-parameters-maximumwaittimewhenidle)
* [DegreeOfReceiveParallelism](/transports/azure-storage-queues/configuration.md#configuration-parameters-degreeofreceiveparallelism)
* [PeekInterval](/transports/azure-storage-queues/configuration.md#configuration-parameters-peekinterval)
* [MessageInvisibleTime](/transports/azure-storage-queues/configuration.md#configuration-parameters-messageinvisibletime)

These values can be set using corresponding extension methods:

```csharp
// For Azure Storage Queues Transport version 11.x
var transport = new AzureStorageQueueTransport(queueServiceClient, blobServiceClient, cloudTableClient)
{
    ReceiverBatchSize = 20,
    MaximumWaitTimeWhenIdle = TimeSpan.FromSeconds(1),
    DegreeOfReceiveParallelism = 16,
    PeekInterval = TimeSpan.FromMilliseconds(100),
    MessageInvisibleTime = TimeSpan.FromSeconds(30)
};

endpointConfiguration.UseTransport(transport);

// For Azure Storage Queues Transport version 10.x
var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
transport.BatchSize(20);
transport.MaximumWaitTimeWhenIdle(TimeSpan.FromSeconds(1));
transport.DegreeOfReceiveParallelism(16);
transport.PeekInterval(TimeSpan.FromMilliseconds(100));
transport.MessageInvisibleTime(TimeSpan.FromSeconds(30));
transport.UseQueueServiceClient(queueServiceClient);
transport.UseBlobServiceClient(blobServiceClient);
transport.UseCloudTableClient(cloudTableClient);

// For Azure Storage Queues Transport version 9.x
var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
transport.BatchSize(20);
transport.MaximumWaitTimeWhenIdle(TimeSpan.FromSeconds(1));
transport.DegreeOfReceiveParallelism(16);
transport.PeekInterval(TimeSpan.FromMilliseconds(100));
transport.MessageInvisibleTime(TimeSpan.FromSeconds(30));
transport.UseQueueServiceClient(queueServiceClient);
transport.UseBlobServiceClient(blobServiceClient);
transport.UseCloudTableClient(cloudTableClient);

// For Azure Storage Queues Transport version 8.x
var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
transport.BatchSize(20);
transport.MaximumWaitTimeWhenIdle(TimeSpan.FromSeconds(1));
transport.DegreeOfReceiveParallelism(16);
transport.PeekInterval(TimeSpan.FromMilliseconds(100));
transport.MessageInvisibleTime(TimeSpan.FromSeconds(30));

// For Azure Storage Queues Transport version 7.x
var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
transport.ConnectionString("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
transport.BatchSize(20);
transport.MaximumWaitTimeWhenIdle(TimeSpan.FromSeconds(1));
transport.DegreeOfReceiveParallelism(16);
transport.PeekInterval(TimeSpan.FromMilliseconds(100));
transport.MessageInvisibleTime(TimeSpan.FromSeconds(30));
```

### PurgeOnStartup

The `PurgeOnStartup` setting now can be set on `EndpointConfiguration` using an extension method.

```csharp
endpointConfiguration.PurgeOnStartup(true);
```

### DefaultQueuePerInstance

The `DefaultQueuePerInstance` setting is deprecated.

### Default value changes

The default values of the following settings have been changed:

* [ConnectionString](/transports/azure-storage-queues/configuration.md#configuration-parameters-connectionstring), which had a default value of `UseDevelopmentStorage=true`, was removed and became mandatory.
* [BatchSize](/transports/azure-storage-queues/configuration.md#configuration-parameters-batchsize) changed from 10 to 32.

## Serialization

In previous versions of the Azure Storage Queues transport, change the default `SerializationDefinition` to `JsonSerializer`.

In version 6 of NServiceBus, transports don't have the ability to manipulate serialization. To preserve backward compatibility and ensure that message payloads are small, setting JSON serialization should now be done on the endpoint configuration level.

```csharp
endpointConfiguration.UseSerialization<JsonSerializer>();
endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
```

## API Changes

In version 7 of the Azure Storage Queues transport, the public API has been reduced. As a result, multiple classes that used to be public in version 6 and below were marked as obsolete with a comment:

> This class served only internal purposes without providing any extensibility point and as such was removed from the public API. For more information, refer to the documentation.

If code exists that depends on classes that were deprecated with the above message, and it is not clear how to update it, [contact Particular support](https://particular.net/contactus) to get help in resolving the issue.
