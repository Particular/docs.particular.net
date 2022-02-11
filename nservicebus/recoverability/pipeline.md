---
title: Recoverability pipeline
summary: Extending the recoverability pipeline with custom behaviors
reviewed: 2022-02-10
component: Core
versions: '[8.0,)'
---

The recoverability [pipeline](/nervicebus/pipeline) allows for advanced customization of the metadata captured during message failures as well as full control over the recoverability action taken.

The example below shows how to [register a behavior](/nservicebus/pipeline/manipulate-with-behaviors.md#add-a-new-step) that:

- Stores the message body in an external storage
- Excludes the body from the message sent to the error queue
- Adds a metadata entry that links to the stored body

snippet: custom-recoverability-action
