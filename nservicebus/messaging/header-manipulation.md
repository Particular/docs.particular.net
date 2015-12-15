---
title: Manipulating message headers
summary: Access and manipulate the built in NServiceBus headers or add custom headers.
tags:
- Header
redirects:
- nservicebus/how-do-i-get-technical-information-about-a-message
related:
- samples/header-manipulation
- nservicebus/messaging/headers
- nservicebus/pipeline/message-mutators
- nservicebus/pipeline/customizing
- nservicebus/handlers
- nservicebus/sagas
---

The mechanism for [header communication](/nservicebus/messaging/headers.md). is either native headers, if the transport supports that feature, or via a serialized collection of key value pairs. This article covers the various ways of manipulating the message headers.


## Reading incoming Headers

Headers can be read for an incoming message.


### From a Behavior

snippet:header-incoming-behavior


### From a Mutator

snippet:header-incoming-mutator


### From a Handler

snippet:header-incoming-handler


### From a Saga

<!-- import header-incoming-saga-->


## Writing outgoing Headers

Headers can be written for an outgoing message.


### From a Behavior

snippet:header-outgoing-behavior


### From a Mutator

snippet:header-outgoing-mutator


### From a Handler

snippet:header-outgoing-handler


### From a Saga

snippet:header-outgoing-saga


### For all outgoing messages

NServiceBus allows you to register headers at configuration time that's then added to all outgoing messages for the endpoint.

<!-- import header-static-endpoint -->


WARNING: In Versions 3 through Version 5 the global outgoing headers are not thread safe. It is recommended that you manipulate them at startup. This has been limited to a configuration time only API in Version 6 .
