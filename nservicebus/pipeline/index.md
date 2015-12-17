---
title: Message Handling Pipeline
summary: Overview of the message handling pipeline
tags:
- Pipeline
redirects:
- nservicebus/nservicebus-pipeline-intro
related:
- samples/header-manipulation
---

NServiceBus has the concept of a "pipeline" which refers to the series of actions taken when an incoming message is process and an outgoing message is sent.

### Customizing the pipeline

There are several ways to customize this pipeline with varying levels of complexity.

 * [Custom behaviors](/nservicebus/pipeline/customizing.md)
 * [Message Mutators](/nservicebus/pipeline/message-mutators.md)
 * [Aborting the Pipeline](/nservicebus/pipeline/aborting.md)

### Features build on the pipeline

 * [DataBus](/nservicebus/messaging/databus.md)
 * [Encryption](/nservicebus/security/encryption.md)
 * [Second Level Retries](/nservicebus/errors/automatic-retries.md)
  
