---
title: Versioning
summary: Message evolution over time using interfaces.
reviewed: 2025-10-14
component: Core
redirects:
- nservicebus/versioning-sample
related:
- nservicebus/messaging/messages-as-interfaces
- nservicebus/messaging/immutable-messages
---

This sample shows how to handle message schema evolution in a backward-compatible manner. The project consists of a publishing endpoint that has evolved from one version of the schema to the next. The newer subscriber has access to the additional information in the newest version of the schema while the older keeps operating without interruptions.

In this sample, there are two versions of the message contract project, `Contracts`. The V1 version of the project is referenced by `V1.Subscriber`, and the V2 version of the project is referenced by `Publisher` and `V2.Subscriber`.

The version 1 message:

snippet: V1Message

The version 2 message inherits from version 1 as shown below, adding an additional property to the message.

snippet: V2Message

Subscribers have a message handler for the messages from their respective versions.

`Publisher` is publishing a version 2 message, however, `V1.Subscriber` receives these messages as well.
