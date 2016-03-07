---
title: Customizing the Pipeline
summary: Customizing the message handling pipeline
tags:
- Pipeline
redirects:
- nservicebus/pipeline/customizing
related:
- nservicebus/pipeline/customizing-v5
- nservicebus/pipeline/customizing-v6
---

NServiceBus has always had the concept of a pipeline execution order that is executed when a message is received or dispatched. From NServiceBus Version 5 on, this pipeline has been made a first level concept and exposes it for extensibility. This allows users to take full control of the incoming and outgoing message processing.

Each pipeline is composed of "steps". A step is an identifiable value in the pipeline used to programmatically define order of execution. Each step represents a behavior which will be executed at the given place within the pipeline. To add additional behavior to the pipeline by registering a new step or replace the behavior of an existing step with the custom implementation.