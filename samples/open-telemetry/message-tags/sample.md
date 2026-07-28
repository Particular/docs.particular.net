---
title: Capturing message properties as OpenTelemetry span tags
summary: Demonstrates how to use pipeline behaviors to capture specific values from message body properties and headers as tags on OpenTelemetry spans
reviewed: 2026-07-27
component: Core
related:
- nservicebus/operations/opentelemetry
---

This sample shows how to use pipeline behaviors to capture specific values from message body properties and message headers as tags on OpenTelemetry spans.

## Running the project

The code consists of a single endpoint that sends messages to itself. Press <kbd>S</kbd> to place an order. As the message is processed, the `OrderId`, `CustomerId`, and `Priority` values appear as tags on the `process` span in the console output.

## Why use behaviors

NServiceBus does not capture message body content as span tags by default. Message payloads can contain sensitive or encrypted data, and adding entire payloads as tags is costly. Pipeline behaviors let the endpoint author explicitly choose which values to expose, at the appropriate pipeline stage.

> [!NOTE]
> Always check `Activity.Current` for `null` before adding tags, or use the null-conditional operator (`?.`). When no trace listeners are registered, `Activity.Current` is `null`.

## Code walkthrough

### Global configuration

partial: enableotel

OpenTelemetry is configured to export traces to the console.

snippet: open-telemetry-config

### Registering behaviors

Both behaviors are registered when the endpoint is configured:

snippet: register-behaviors

### Capturing message body properties

Message body properties are only available after deserialization. The behavior runs at the `IIncomingLogicalMessageContext` stage, where the deserialized message instance is accessible via `context.Message.Instance`.

snippet: capture-message-property-tags

The behavior casts the message instance to the expected type. If the cast succeeds, specific properties are added as tags on the current span. The tags appear on the `process` span in the trace.

### Capturing header values

Headers are available before deserialization at the `IIncomingPhysicalMessageContext` stage. The behavior reads a specific header by name.

snippet: capture-header-tags

The sender sets the header on the outgoing message:

snippet: set-custom-header
