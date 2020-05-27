---
title: Building a Custom Feature
summary: How to build a custom feature in NServiceBus
reviewed: 2019-11-25
component: Core
related:
- nservicebus/pipeline
- nservicebus/pipeline/features
---

This sample illustrates how to build a custom [NServiceBus feature](/nservicebus/pipeline/features.md). In this feature, some diagnostics are performed:

 * Logging [Handler](/nservicebus/handlers/) times.
 * Logging [Saga](/nservicebus/sagas/) data state.

Both of these are implemented as dependent features that depend on the diagnostics feature.


## Diagnostics feature

snippet: DiagnosticsFeature

The diagnostics feature allows all dependencies to be easily toggled, enabling or disabling them through configuration. In this case it is enabled by default.


### Custom logger

This feature injects a custom logger that can be used by other features.

snippet: CustomLogger


## Handler timing feature

This feature depends on the diagnostics feature.

snippet: HandlerTimerFeature


### Behavior

The [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) that performs the handler timing.

snippet: HandlerTimerBehavior


## Saga State Audit Feature

This feature depends on both the Diagnostics and [Saga](/nservicebus/sagas/) Features.

snippet: SagaStateAuditFeature


### Behavior

The [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) that captures the saga state.

snippet: SagaStateAuditBehavior