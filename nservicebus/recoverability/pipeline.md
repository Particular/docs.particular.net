---
title: Recoverability pipeline
summary: Extending the recoverability pipeline with custom behaviors
reviewed: 2024-10-24
component: Core
versions: '[8.0,)'
---

The recoverability [pipeline](/nservicebus/pipeline/) allows for advanced customization of the metadata captured during message failures as well as full control over the recoverability action taken.

The example below shows how to extend the pipeline with a behavior that:

- Stores the message body in an external storage
- Excludes the body from the message sent to the error queue
- Adds a metadata entry that links to the stored body

snippet: custom-recoverability-action

In addition, the behavior must be [registered in the pipeline](/nservicebus/pipeline/manipulate-with-behaviors.md#add-a-new-step).
