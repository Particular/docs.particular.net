---
title: Observability
summary: Tracing Non-Durable Transport operations with OpenTelemetry
reviewed: 2026-07-31
component: NonDurableTransport
related:
- nservicebus/operations/opentelemetry
---

The transport emits OpenTelemetry activities under the source `NServiceBus.Transport.NonDurable`.

| Activity name | Kind | Description |
|:---|:---|:---|
| `NServiceBus.Transport.NonDurable.Send` | Producer | A message is dispatched to a destination queue. |
| `NServiceBus.Transport.NonDurable.Schedule` | Producer | A delayed delivery message is scheduled. |
| `NServiceBus.Transport.NonDurable.Process` | Consumer | A message is received and processed. |

Tags include `messaging.system = nondurable`, `messaging.destination.name`, `messaging.operation.name`, `messaging.message.id`, and `messaging.message.conversation_id`.
