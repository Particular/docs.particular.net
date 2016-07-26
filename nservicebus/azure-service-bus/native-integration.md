---
title: Azure Service Bus Transport Native Integration
tags:
- Azure
- Cloud
---


## Versions 6 and below


### BrokeredMessage body conventions

By default `BrokeredMessage` body is transmitted as a byte array. But for scenarios such as native integration, the body can be stored and retrieved using `Stream`. To specify how the `BrokeredMessage` body is stored and retrieved, override conventions provided by using `BrokeredMessageBodyConversion` class.

Outgoing message:

snippet: ASB-outgoing-message-convention

Incoming message:

snippet: ASB-incoming-message-convention
