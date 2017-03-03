---
title: Message Handling Pipeline
summary: Overview of the message handling pipeline
reviewed: 2016-11-05
tags:
- Pipeline
redirects:
- nservicebus/nservicebus-pipeline-intro
related:
- samples/header-manipulation
---

NServiceBus has the concept of a "pipeline" which refers to the series of actions taken when an incoming message is processed and an outgoing message is sent.

### Customizing the pipeline

There are several ways to customize this pipeline with varying levels of complexity.

 * [Manipulate the Pipeline with Behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md)
 * [Steps, Stages and Connectors](/nservicebus/pipeline/steps-stages-connectors.md)
 * [Message Mutators](/nservicebus/pipeline/message-mutators.md)
 * [Aborting the Pipeline](/nservicebus/pipeline/aborting.md)
 * [Reacting to pipeline events](/nservicebus/pipeline/events.md)

### Features build on the pipeline

 * [DataBus](/nservicebus/messaging/databus/)
 * [Message Property Encryption](/nservicebus/security/property-encryption.md)
 * [Recoverability](/nservicebus/recoverability/)
