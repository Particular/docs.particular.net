---
title: Non-Durable Transport Usage
summary: Demonstrates two co-located endpoints sharing a non-durable broker, with inline execution, non-durable persistence, and saga timeouts
reviewed: 2026-07-30
component: NonDurableTransport
related:
 - transports/non-durable
 - persistence/non-durable
---

This sample demonstrates two NServiceBus endpoints running in the same process, both using the Non-Durable Transport and Non-Durable Persistence. They share a single `NonDurableBroker` instance so messages flow between them in memory.

| Endpoint | Inline Execution | Handlers |
|:---|:---|:---|
| SagaEndpoint | Enabled | `OrderSaga`, `PaymentCompletedHandler` |
| PaymentEndpoint | Disabled | `ProcessPaymentHandler` |

## What the sample demonstrates

- **Shared broker** — Both endpoints use the same `NonDurableBroker` so they communicate without external infrastructure.
- **Inline execution** — SagaEndpoint enables inline execution. When the saga sends a message to its own queue via `SendLocal`, the handler runs synchronously in the saga's thread.
- **Normal async messaging** — Events published by PaymentEndpoint flow through the shared broker and are picked up by SagaEndpoint's pump.
- **Transaction sharing** — With both transport and persistence non-durable and `SendsAtomicWithReceive` (the default), saga updates and outgoing messages commit atomically.
- **Multi-hosting** — Two endpoints in one process using keyed dependency injection.

## Prerequisites

- No external infrastructure required. The sample runs entirely in memory.

## Running the sample

 1. Run the console application.
 1. Press <kbd>Enter</kbd> to send a `PlaceOrder` message.
 1. Observe the console output as the saga starts, sends `ProcessPayment` to PaymentEndpoint, and handles the `PaymentCompleted` event or a timeout.

## Code walk-through

### Endpoint configuration

snippet: sample-config

Both endpoints disable assembly scanning and explicitly register their handlers and sagas, which is required for [multi-endpoint hosting](/nservicebus/hosting/core-hosting.md#hosting-multiple-endpoints). Each endpoint has its own `NonDurableTransport` instance, but they share the same `NonDurableBroker`. SagaEndpoint enables inline execution; PaymentEndpoint does not.

### The saga

snippet: thesaga

`OrderSaga` is started by a `PlaceOrder` message. It sends a `ProcessPayment` command to PaymentEndpoint and requests a `CancelOrder` timeout. When the `PaymentCompleted` event arrives, the saga completes. If the timeout fires first, the saga completes anyway.

## Expected output

When <kbd>Enter</kbd> is pressed, the console shows output similar to:

```
Sent PlaceOrder with OrderId <guid>

PlaceOrder received with OrderId <guid>
Sending ProcessPayment to PaymentEndpoint
Requesting CancelOrder timeout in 30 seconds
Processing payment for OrderId <guid>
PaymentCompletedHandler received event for OrderId <guid>
PaymentCompleted received with OrderId <guid>. Completing saga.
```

If the endpoint is stopped before the `PaymentCompleted` event arrives, the `CancelOrder` timeout will fire instead:

```
CancelOrder timeout fired for OrderId <guid>. Completing saga.
```
