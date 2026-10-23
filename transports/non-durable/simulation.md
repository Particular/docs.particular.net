---
title: Simulation and rate limiting
summary: Simulate delays, rejections, and rate limits with the Non-Durable Transport
reviewed: 2026-07-31
component: NonDurableTransport
---

The `NonDurableBroker` supports optional simulation settings for testing resilience scenarios. Simulation can introduce artificial delays or rejections on send, receive, and delayed delivery operations. Combined with a fake time provider, tests can simulate the passage of time deterministically.

## Basic simulation

Configure rate limiting with a fixed window:

snippet: NonDurableTransport-Simulation

## Simulating the passage of time

Use `FakeTimeProvider` to advance time deterministically in tests:

snippet: NonDurableTransport-SimulatedTime

## Per-queue rate limit overrides

Apply different simulation settings to specific queues:

snippet: NonDurableTransport-QueueOverride

## Delayed delivery simulation

Control the timing of delayed message delivery:

snippet: NonDurableTransport-DelayedDeliverySimulation
