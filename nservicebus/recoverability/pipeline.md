---
title: Recoverability pipeline
summary: Extending the recoverability pipeline with custom behaviors
reviewed: 2022-02-10
component: Core
versions: '[8.0,)'
---

The recoverability [pipeline](/nervicebus/pipeline) allows advanced customization of metadata captured during message failures and also full control over the recoverability action taken.

The example below shows how to [register a behavior](TODO) that:

- Stores the message body separately
- Excludes it from the message sent to the error queue
- Adds a metadata entry that links to the stored body

snippet: custom-recoverability-action
