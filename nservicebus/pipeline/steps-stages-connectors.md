---
title: Steps, Stages and Connectors
summary: The pipeline is composed of a number of Stages that communicate via Connectors
component: Core
reviewed: 2020-12-07
related:
- nservicebus/pipeline/manipulate-with-behaviors
---

partial: intro

partial: stages

partial: incoming

partial: outgoing

partial: recoverability

partial: optional

partial: extensionbag

### Send options

Settings configured on `SendOptions` (etc.) instances are internally stored in a dedicated `ContextBag`. These settings are accessibe within the pipeline via the `context.GetOperationProperties()` extension.

var sendOptions = new SendOptions();
sendOptions.GetExtensions().Set(somevalue);

...

context.GetOperationProperties().Get(somevalue)

partial: connectors

