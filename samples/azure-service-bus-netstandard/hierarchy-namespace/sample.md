---
title: Azure Service Bus Hierarchy Namespace Sample
summary: Demonstrates how to use hierarchical namespaces with Azure Service Bus
reviewed: 2026-02-10
component: ASBS
related:
- transports/azure-service-bus/configuration
---

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

This sample shows a usage of hierarchy namespace with Azure Service Bus. In the sample there are three endpoints:
- `HierarchyClient` - belonging to a hierarchy (`my-hierarchy`), sends messages and publishes an event,
- `HierarchyEndpoint` - belonging to the same hierarchy, handles messages and subscribes to the event,
- `ExternalEndpoint` - outside of the hierarchy, handles messages and subscribes to the event.

The `HierarchyClient` is going to send the following messages
- Commands
  - `HierarchyCommand`,
  - `ExternalCommand`, that is excluded from the hierarchy with the following code:
- Events
  - `HierarchyEvent`,
  - `ExternalEvent` and `OtherExternalEvent`, both implementing the `IExternalEvent` interface.

The message types are excluded from the hierarchy with the following code:

snippet: excludedMessage

The command is excluded explicitly (by using its type), but events are excluded with their interfaces (so that one line excludes all events under `IExternalEvent`).

The sample covers the following scenarios:

### Scenario 1 - hierarchy message

`HierarchyClient` sends a `HierarchyCommand` to both endpoints. It is sent successfully to `HierarchyEndpoint`, because it belongs to the same hierarchy. It is not sent to `ExternalEndpoint`, because it is outside of the hierarchy. The following message is included in the exception:

`The messaging entity 'sb://xxx.servicebus.windows.net/my-hierarchy/Samples.ASBS.HierarchyNamespace.ExternalEndpoint' could not be found.`

### Scenario 2 - excluded message

`HierarchyClient` sends an `ExternalCommand` to both receivers. It is sent successfully to `ExternalEndpoint`, because this type of message is excluded from the hierarchy and `ExternalEndpoint` is outside of the hierarchy. It is not sent to `HierarchyEndpoint`, because it is in the hierarchy. The following message is included in the exception:

`The messaging entity 'sb://xxx.servicebus.windows.net/Samples.ASBS.HierarchyNamespace.HierarchyEndpoint' could not be found.`

### Scenario 3 - hierarchy event

`HierarchyClient` publshes a `HierarchyEvent`. Both receivers are subscribed to this type of event. Only `HierarchyEndpoint` receives it though, because it is in the same hierarchy as `HierarchyClient`. Since `ExternalEndpoint` is outside of the hierarchy, that event is not delivered there.

 Note that there are no exceptions while publishing an event, even though it is not delivered to `ExternalEndpoint` that has a handler for this kind of events.

 ### Scenario 4 - hierarchy event

`HierarchyClient` publshes both a `ExternalEvent` and `OtherExternalEvent`. Both endpoints are subscribed to this type of event. Only `ExternalEndpoint` receives them though, because it is in the same hierarchy as `HierarchyClient`. Since `HierarchyEndpoint` is in the hierarchy, those event are not delivered there.

### Transport configuration

snippet: config

Two endpoints (`HierarchyClient` and `HierarchyEndpoint` are configured to be included in the hierarchy `my-hierarchy`). `ExternalEndpoint` is not included in the hierarchy.

snippet: namespaceOptions

This is necessary to include an endpoint in a hierarchy.

## Viewing messages in-flight

The following queues and topics for the endpoints can be seen in the Azure Portal or a third-party tool:

- `my-hierarchy/samples.asbs.hierarchynamespaceescape.hierarchyclient`
- `my-hierarchy/samples.asbs.hierarchynamespaceescape.hierarchyendpoint`
- `samples.asbs.hierarchynamespaceescape.externalendpoint`
- `my-hierarchy/shared.hierarchyevent`
- `my-hierarchy/shared.externalevent`
- `my-hierarchy/shared.otherexternalevent`
- `hierarchyevent`
- `shared.externalevent`
- `shared.otherexternalevent`
- `error`
