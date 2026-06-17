---
title: Azure Storage Queues Transport Upgrade Version 14 to 15
summary: Instructions on how to upgrade the Azure Storage Queues transport from version 14 to 15.
reviewed: 2026-06-17
component: ASQ
related:
- transports/azure-storage-queues
isUpgradeGuide: true
---

## Message-driven publish/subscribe deprecation

Customers who are currently using message-driven publish/subscribe compatibility should migrate to native publish/subscribe instead. Compatibility functionality is marked as obsolete with a warning for this version and will be treated as an error starting in the next major release.

### Migration Steps

Assuming a transport configuration setup similar to:

```csharp
async Task Initialize()
{
    var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

    // Rest of your transport configuration here
    var transport = new AzureStorageQueueTransport("connectionString");
    // ...

    // Enable message-driven publish/subscribe compatibility
    transport.EnableMessageDrivenPubSubCompatibilityMode();

    var routing = endpointConfiguration.UseTransport(transport);
    routing.RegisterPublisher(
        assembly: typeof(MyEvent).Assembly,
        publisherEndpoint: "Publisher");
}
```

The following steps need to be taken for all event types that require migration:

- Disable auto-subscribe for each event type
- Unsubscribe from each event type using the endpoint instance unsubscribe API
- Remove usage of both `RegisterPublisher` and `EnableMessageDrivenPubSubCompatibilityMode`.

An example of how this may be done is as follows:

```csharp
async Task Initialize(bool migrateAwayFromMessageDrivenPubSub = false)
{
    if (migrateAwayFromMessageDrivenPubSub)
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

    // Enable message-driven publish/subscribe compatibility
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
