---
title: Handlers and Sagas
summary: Introduction to Handlers and Sagas
component: Core
reviewed: 2016-09-27
related:
 - nservicebus/handlers
 - nservicebus/sagas
 - samples/saga
---

There are two standards ways of executing code when a message is processed: [Handlers](handlers/) and [Sagas](sagas/).

Handler instances are instantiated on a per message basis, executed and then disposed of.

Saga instances are also instantiated on a per message basis, executed and then disposed of. However they differ from Handlers in that they once instantiated they are passed an instance of a "Data" class. The "Saga Data" is persistent state that is shared between a given saga type based on a key.

Other concepts that both Handlers and Sagas share

 * [Recoverability](/nservicebus/recoverability/). i.e. what happens when message processing fails.
 * Executed within the same [Pipeline](/nservicebus/pipeline).
 * Detected via [Assembly scanning](/nservicebus/hosting/assembly-scanning.md).
