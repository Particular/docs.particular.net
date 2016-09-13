---
title: Pipeline Steps, Stages and Connectors
summary: The pipeline is composed of a number of Stages that communicate via Connectors
component: Core
reviewed: 2016-08-23
versions: '[5.0,)'
tags:
- Pipeline
related:
- nservicebus/pipeline/manipulate-with-behaviors
---

NServiceBus has the concept of a pipeline execution order that is executed when a message is received or dispatched. A *pipeline* refers to the series of actions taken when an incoming message is processed or an outgoing message is sent. From NServiceBus Version 5 and above, this pipeline has been made a first level concept and exposed for extensibility. This allows users to take full control of the incoming and outgoing message processing.

In NServiceBus there are two explicit pipelines: one for the outgoing messages and one for the incoming messages.

Each pipeline is composed of *steps*. A step is an identifiable value in the pipeline used to programmatically define order of execution. Each step represents a behavior which will be executed at the given place within the pipeline. To add additional behavior to the pipeline by registering a new step or replace the behavior of an existing step with the custom implementation.

partial: extra
