---
title: Message Handling Pipeline
summary: Overview of the message handling pipeline
reviewed: 2020-05-07
redirects:
- nservicebus/nservicebus-pipeline-intro
related:
- samples/header-manipulation
- samples/pipeline/fix-messages-using-behavior
---

NServiceBus has the concept of a _pipeline_ which refers to the series of actions taken when an incoming message is processed and an outgoing message is sent.

## Customizing the pipeline

There are several ways to customize this pipeline with varying levels of complexity.

* [Manipulate the pipeline with behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md)
* [Steps, stages and connectors](/nservicebus/pipeline/steps-stages-connectors.md)
* [Message mutators](/nservicebus/pipeline/message-mutators.md)
* [Abort the pipeline](/nservicebus/pipeline/aborting.md)
* [React to pipeline events](/nservicebus/pipeline/events.md)

 Unit testing of custom extensions is supported by [the `NServiceBus.Testing` library](/nservicebus/testing/#testing-a-behavior).

## Features built using the pipeline

* [Data Bus](/nservicebus/messaging/databus/)
* [Message Property Encryption](/nservicebus/security/property-encryption.md)
* [Recoverability](/nservicebus/recoverability/)