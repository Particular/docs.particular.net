---
title: Azure Storage Queues Transport Upgrade Version 14 to 14.1
summary: Instructions on how to upgrade the Azure Storage Queues transport from version 14 to 14.1.
reviewed: 2026-06-04
component: ASQ
related:
- transports/azure-storage-queues
isUpgradeGuide: true
---

## Message-driven pubsub deprecation

Customers who are currently using message-driven pubsub compatibility should migrate to native pubsub instead. Compatibility functionality is marked as obsolete with a warning for this version and will be treated as an error starting in the next major release.

### Migration Steps

Assuming a transport configuration setup similar to:

```csharp
async Task Initialize()
{
    var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

    // Rest of your transport configuration here
    var transport = new AzureStorageQueueTransport("connectionString");
    // ...

    // Enable message-driven pubsub compatibility
    transport.EnableMessageDrivenPubSubCompatibilityMode();

    var routing = endpointConfiguration.UseTransport(transport);
    routing.RegisterPublisher(
        assembly: typeof(MyEvent).Assembly,
        publisherEndpoint: "Publisher");
}
```

The following steps need to be taken for all event types that require migration:

- Disable auto-subscribe for each event type
- Unsubscribe the message-driven pubsub for each event type
- Remove usage of both `RegisterPublisher` and `EnableMessageDrivenPubSubCompatibilityMode`

An example of how this may be done is as follows:

```csharp
async Task Initialize(bool migrate = false)
{
    if (migrate)
    {
        await MigrateAwayFromMessageDrivenPubSub();
    }

    var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

    // Rest of your transport configuration here
    var transport = new AzureStorageQueueTransport("connectionString");
    // ...

    var routingConfig = config.UseTransport(transport);
}

async Task MigrateAwayFromMessageDrivenPubSub()
{
    var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

    // Disable auto-subscribe only for events that need to be migrated
    var autoSubscribe = config.AutoSubscribe();
    autoSubscribe.DisableFor<MyEvent>();

    // Rest of your transport configuration here
    var transport = new AzureStorageQueueTransport("connectionString");
    // ...

    // Enable message-driven pubsub compatibility
    transport.EnableMessageDrivenPubSubCompatibilityMode();

    var routing = endpointConfiguration.UseTransport(transport);
    routing.RegisterPublisher(
        assembly: typeof(MyEvent).Assembly,
        publisherEndpoint: "Publisher");

    var endpointInstance = await Endpoint.Start(endpointConfiguration);

    // Unsubscribe only for events that need to be migrated
    await endpointInstance.Unsubscribe<MyEvent>();

    await endpointInstance.Stop();
}
```
