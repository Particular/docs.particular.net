---
title: Building a custom feature
summary: How to build a custom feature in NServiceBus
reviewed: 2024-12-24
component: Core
related:
- nservicebus/pipeline
- nservicebus/pipeline/features
---

This sample illustrates how to build a custom [NServiceBus feature](/nservicebus/pipeline/features.md). In this feature, some diagnostics are performed:

* Logging the time it takes to process messages in a [message handler](/nservicebus/handlers/)
* Logging the state of [sagas](/nservicebus/sagas/)

Both of these are implemented as features that depend on the diagnostics feature.

## Diagnostics feature

snippet: DiagnosticsFeature

The diagnostics feature allows all dependencies to be easily toggled, enabling or disabling them through configuration. In this case, it is enabled by default.

### Custom logger

The feature in this sample injects a custom logger that can be used by other features:

snippet: CustomLogger

## Handler timing feature

The feature depends on the diagnostics feature:

snippet: HandlerTimerFeature

### Behavior

The [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) that performs the handler timing:

snippet: HandlerTimerBehavior

## Saga state audit feature

This feature depends on both the Diagnostics and [Saga](/nservicebus/sagas/) features.

snippet: SagaStateAuditFeature

### Behavior

The [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) that captures the saga state:

snippet: SagaStateAuditBehavior
