---
title: Message Handling Pipeline
summary: Overview of the message handling pipeline
reviewed: 2025-04-08
component: Core
redirects:
- nservicebus/nservicebus-pipeline-intro
related:
- samples/header-manipulation
- samples/pipeline/fix-messages-using-behavior
---

partial: intro

## Pipeline customization

There are several ways to customize the pipelines with varying levels of complexity.

* [Manipulate the pipeline with behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md)
* [Steps, stages and connectors](/nservicebus/pipeline/steps-stages-connectors.md)
* [Message mutators](/nservicebus/pipeline/message-mutators.md)
* [Abort the pipeline](/nservicebus/pipeline/aborting.md)
* [React to pipeline events](/nservicebus/pipeline/events.md)

Unit testing of custom extensions is supported by [the `NServiceBus.Testing` library](/nservicebus/testing/#testing-a-behavior).
