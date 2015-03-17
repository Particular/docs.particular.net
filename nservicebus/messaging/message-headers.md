---
title: Message Headers
summary: Access and manipulate the built in NServiceBus headers or add custom headers.
tags: []
redirects:
- nservicebus/how-do-i-get-technical-information-about-a-message
redirects:
- nservicebus/message-headers
---

Extra information about a message is communicated over the transport in a serialized collection of key value pairs in a similar way to how http headers are used.

### Reading incoming Headers

Headers can be read for an incoming message.

#### From a Behavior

<!-- import header-incoming-behavior -->

#### From a Mutator

<!-- import header-incoming-mutator -->

#### From a Handler

<!-- import header-incoming-handler -->

### Reading outgoing Headers

Headers can be written for an outgoing message.

#### From a Behavior

<!-- import header-outgoing-behavior -->

#### From a Mutator

<!-- import header-outgoing-mutator -->

#### From a Handler

<!-- import header-outgoing-handler -->
