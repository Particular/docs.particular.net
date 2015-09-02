---
title: Manipulating message headers
summary: Access and manipulate the built in NServiceBus headers or add custom headers.
tags: 
- Header
redirects:
- nservicebus/how-do-i-get-technical-information-about-a-message
related:
- samples/header-manipulation
- nservicebus/messaging/message-headers
---

Extra information about a message is communicated over the transport as secondary information to the message body. The mechanism for header communication is either native headers, if the transport supports that feature, or via a serialized collection of key value pairs. Message headers are very similar, in both implementation and usage, to http headers. This article covers the various ways of manipulating the message headers. To learn more about the built-in headers, see [that article](/nservicebus/messaging/message-headers.md).


## Reading incoming Headers

Headers can be read for an incoming message.


### From a Behavior

<!-- import header-incoming-behavior -->


### From a Mutator

<!-- import header-incoming-mutator -->


### From a Handler

<!-- import header-incoming-handler -->


## Writing outgoing Headers

Headers can be written for an outgoing message.


### From a Behavior

<!-- import header-outgoing-behavior -->


### From a Mutator

<!-- import header-outgoing-mutator -->


### From a Handler

<!-- import header-outgoing-handler -->


### For all outgoing messages

NServiceBus allows you to register headers at configuration time that's then added to all outgoing messages for the endpoint.

<!-- import header-static-endpoint --> 


WARNING: In Versions 3 through Version 5 the global outgoing headers are not thread safe. It is recommended that you manipulate them at startup. This has been limited to a configuration time only API in Version 6 .