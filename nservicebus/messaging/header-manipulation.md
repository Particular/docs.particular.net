---
title: Manipulating message headers
summary: Access and manipulate the built-in NServiceBus headers or add custom headers
reviewed: 2020-01-03
component: Core
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

Manipulating custom headers can be useful for storing infrastructure-level information not directly related to the business message. Instead of forcing all message types to inherit from a base class or implement a specific interface in order to force the existence of certain properties, instead consider moving this information into message headers.

Message headers are best manipulated through [pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md), however they can be accessed and modified from message handlers and saga handlers as well.

Depending on the [message transport](/transports/), headers are stored with the message either as native headers (if supported) or via a serialized collection of key/value pairs within the message body itself.

This article covers the various ways of manipulating the message headers.


## Reading incoming headers

Headers can be read for an incoming message.


### From a behavior

snippet: header-incoming-behavior


### From a mutator

snippet: header-incoming-mutator


### From a handler

snippet: header-incoming-handler


### From a saga

snippet: header-incoming-saga


## Writing outgoing headers

Headers can be written for an outgoing message.


### From a behavior

snippet: header-outgoing-behavior


### From a mutator

snippet: header-outgoing-mutator


### From a handler

snippet: header-outgoing-handler


### From a saga

snippet: header-outgoing-saga


### For all outgoing messages

NServiceBus supports registering headers at configuration time that are then added to all outgoing messages for the endpoint.

snippet: header-static-endpoint

partial: threadsafe