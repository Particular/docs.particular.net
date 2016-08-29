---
title: Manipulating message headers
summary: Access and manipulate the built in NServiceBus headers or add custom headers.
reviewed: 2016-08-29
component: Core
tags:
- Header
redirects:
- nservicebus/how-do-i-get-technical-information-about-a-message
related:
- samples/header-manipulation
- nservicebus/messaging/headers
- nservicebus/pipeline/message-mutators
- nservicebus/pipeline
- nservicebus/handlers
- nservicebus/sagas
---

The mechanism for [header communication](/nservicebus/messaging/headers.md) is either native headers, if the transport supports that feature, or via a serialized collection of key value pairs. This article covers the various ways of manipulating the message headers.


## Reading incoming Headers

Headers can be read for an incoming message.


partial: incomingbehavior


### From a Mutator

snippet: header-incoming-mutator


### From a Handler

snippet: header-incoming-handler


### From a Saga

snippet: header-incoming-saga


## Writing outgoing Headers

Headers can be written for an outgoing message.


partial: outgoingbehavior


### From a Mutator

snippet: header-outgoing-mutator


### From a Handler

snippet: header-outgoing-handler


### From a Saga

snippet: header-outgoing-saga


### For all outgoing messages

NServiceBus supports registering headers at configuration time that are then added to all outgoing messages for the endpoint.

snippet: header-static-endpoint

partial: threadsafe
