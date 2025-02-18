---
title: Azure Service Bus topology migration
summary: Demonstrates how to migrate from the single-topic to topic-per-event topology
reviewed: 2025-02-12
component: ASBS
related:
- transports/azure-service-bus
---


## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

This sample shows a basic two-endpoint scenario in which one endpoint is publishing an event (Publisher) which the other is processing it (Subscriber). Both endpoints use version 5 of the transport but, initially, are configured with the migration topology to deliver the even using the single-topic approach via the `bundle-1` topic.

## Running the sample

1. First, without changing the code, run the `Subscriber` project by itself. This will create all the necessary publish/subscribe infrastructure in Azure Service Bus.
2. Next, without changing the code, run the `Publisher` project. This will publish an event that the `Subscriber` will process.

Before the event delivery path can be migrated to topic-per-event approach, the infrastructure needs to be created in Azure Service Bus. This can be done using the [command line utility](/transports/azure-service-bus/operational-scripting.md):

```
asb-transport migration endpoint subscribe-migrated Subscriber Shared.MyEvent
```

Alternatively, it can be also done by the endpoint itself during startup if [installers are enabled](/nservicebus/operations/installers.md#running-installers-during-endpoint-startup). To do that comment out the "Step 0" code in the `Program.cs` file of Subscriber and uncomment "Step 1". Run the sample to verify if the event has been delivered. Note that the event has been delivered via the single-topic path.

Now that the infrastructure for the new path has been created, switch the Publisher to use that path:

1. Comment out the "Step 0" code in the `Program.cs` file of Publisher and uncomment "Step 2".
2. Run the sample to verify if the event has been delivered. Note that the event has been delivered via the new path.

To verify that, decommission the old path. You can do that using the [command line utility](/transports/azure-service-bus/operational-scripting.md):

```
asb-transport migration endpoint unsubscribe Subscriber Shared.MyEvent
```

Run the sample to verify if the event has indeed been delivered.

In the last step

1. Comment out the "Step 1" and "Step 2" lines
2. Uncomment "Step 3" in both Publisher and Subscriber

After this change the endpoints are fully migrated to the topic-per-event topology and are no longer able to communicate with endpoint that use the single-topic approach.