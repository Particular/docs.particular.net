---
title: Building a Custom Feature
reviewed: 2016-05-31
component: Core
tags:
- Pipeline
- Feature
related:
- nservicebus/pipeline
- nservicebus/pipeline/features
---

## Introduction

This sample illustrates how to build a custom Feature. In this Feature some diagnostics is performed:

 * Logging [Handler](/nservicebus/handlers/) times.
 * Logging [Saga](/nservicebus/sagas/) data state.

Both of these are implemented as dependent Features that depend on the Diagnostics Feature.


## Diagnostics Feature

snippet: DiagnosticsFeature

The Diagnostics Feature enables all dependent to be easily toggled enabling or disabling it through configuration. In this case it is enabled by default.


### Custom Logger

This Feature injects a custom logger into dependency injection that can then be used by below Features.

snippet: CustomLogger


## Handler Timing Feature

This feature depends on both the Diagnostics Feature.

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