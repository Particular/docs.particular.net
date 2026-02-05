---
title: Customizing OpenTelemetry tracing
summary: Demonstrates how to add data to existing OpenTelemetry traces
reviewed: 2024-07-26
component: Core
related:
- nservicebus/operations/opentelemetry
---

This sample shows how to extend the OpenTelemetry activities in different ways.

## Running the project

The code consists of a single endpoint project that sends messages to itself.

Press <kbd>O</kbd> to send a `CreateOrder` message with a randomized `OrderId`. When the message is handled, two more messages are created: `BillOrder` and `ShipOrder`.

As the messages are sent and processed, trace data is exported to the console. Some of the trace data originates from NServiceBus and some from custom code in the sample.

## Code walk through

### Global configuration

partial: enableotel

OpenTelemetry is configured to export all traces to the command line. It includes the `NServiceBus.*` sources built into NServiceBus and a custom activity source defined in the sample (see below).

snippet: open-telemetry-config

### Custom activities

The sample includes a custom activity source.

snippet: custom-activity-source

The handler for `CreateOrder` includes a custom activity that wraps around the billing section.

snippet: custom-activity-in-handler

This will automatically be created as a child activity of the invoke handler activity created by NServiceBus. The NServiceBus send message activity will treat this custom activity as its parent.

```
Send CreateOrder
  Process CreateOrder
    Invoke CreateOrderHandler
      Billing Order <-- Custom activity
        Send BillOrder
      Send ShipOrder
```

### Adding tags

> [!NOTE]
> `Activity.Current` may be `null` if there are no configured trace listeners. Always check if the value is null before calling methods on an `Activity` instance, or use the null-conditional operator (`?.`).

The handler for `ShipOrder` adds tags to the ambient behavior.

snippet: add-tags-from-handler

In the sample, these tags will be added to the NServiceBus invoke handler activity.

```
Send ShipOrder
  Process ShipOrder
    Invoke ShipOrderHandler <-- Custom tag gets added here
```

A behavior in the outgoing pipeline adds the size of the message as a tag for all outgoing message activities.

snippet: add-tags-from-outgoing-behavior

partial: handler-behavior
