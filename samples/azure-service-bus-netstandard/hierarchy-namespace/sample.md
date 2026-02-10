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

This sample shows a hierarchy namespace with the Azure Service Bus transport. In the sample, there are three endpoints:
- `HierarchyClient` - belongs to a hierarchy (`my-hierarchy`), and sends messages and publishes an event
- `HierarchyEndpoint` - belongs to the same hierarchy, and handles all commands and events, but should only receive those from the hierarchy
- `ExternalEndpoint` - outside of the hierarchy, and handles all commands and events, but should only receive those excluded from the hierarchy

The `HierarchyClient` sends the following messages
- Commands
  - `HierarchyCommand` 
  - `ExternalCommand`
- Events
  - `HierarchyEvent`,
  - `ExternalEvent` and `OtherExternalEvent` (both implement the `IExternalEvent` interface)

### Transport configuration

The two hierarchy endpoints (`HierarchyClient` and `HierarchyEndpoint`) are configured to be included in the hierarchy with the following configuration:

snippet: namespaceOptions

The `ExternalEndpoint` is not included in the hierarchy.

#### Escaping the hierarchy

Messages can be configured to be sent outside of the hierarchy using the `ExcludeMessageType` method on the `HierarchyNamespaceOptions` property of the transport:

snippet: excludedMessage

The command is excluded explicitly (by using its concrete type), and the events are excluded using their shared interface (so that one line excludes all events inheriting `IExternalEvent`).

### Scenario 1 - hierarchy command

`HierarchyClient` sends a `HierarchyCommand` to both endpoints. It is sent successfully to `HierarchyEndpoint` because it belongs to the same hierarchy. It is not sent to `ExternalEndpoint` because it is outside the hierarchy, and the expected queue  does not exist(`my-hierarchy/Samples.ASBS.HierarchyNamespace.ExternalEndpoint` should not be prefixed with `my-hierarchy\`). The following message is included in the exception:

`The messaging entity 'sb://xxx.servicebus.windows.net/my-hierarchy/Samples.ASBS.HierarchyNamespace.ExternalEndpoint' could not be found.`

### Scenario 2 - excluded command

`HierarchyClient` sends an `ExternalCommand` to both receivers. It is sent successfully to `ExternalEndpoint` because this type of message is excluded from the hierarchy, and `ExternalEndpoint` is outside the hierarchy. It is not sent to `HierarchyEndpoint`, because it is in the hierarchy, and the expected queue does not exist (`Samples.ASBS.HierarchyNamespace.HierarchyEndpoint` should be prefixed with `my-hierarchy/`). The following message is included in the exception:

`The messaging entity 'sb://xxx.servicebus.windows.net/Samples.ASBS.HierarchyNamespace.HierarchyEndpoint' could not be found.`

### Scenario 3 - hierarchy event

`HierarchyClient` publishes a `HierarchyEvent`. Both endpoints are subscribed to this event. Only `HierarchyEndpoint` receives it because it is in the same hierarchy as `HierarchyClient`. Since `ExternalEndpoint` is outside the hierarchy, the event is not delivered to it.

 ### Scenario 4 - excluded events

`HierarchyClient` publishes both `ExternalEvent` and `OtherExternalEvent` events. Both endpoints are subscribed to these events. Only `ExternalEndpoint` receives them because it is outside of the `my-hierarchy` hierarchy, and these events have been excluded.

## Viewing messages in-flight

The following queues and topics for the endpoints can be seen in the Azure Portal or a third-party tool:

### Queues

- `my-hierarchy/samples.asbs.hierarchynamespaceescape.hierarchyclient`
- `my-hierarchy/samples.asbs.hierarchynamespaceescape.hierarchyendpoint`
- `samples.asbs.hierarchynamespaceescape.externalendpoint`
- `my-hierarchy/error`
- `error`

### Topics

- `my-hierarchy/shared.hierarchyevent`
- `my-hierarchy/shared.externalevent`
- `my-hierarchy/shared.otherexternalevent`
- `hierarchyevent`
- `shared.externalevent`
- `shared.otherexternalevent`
