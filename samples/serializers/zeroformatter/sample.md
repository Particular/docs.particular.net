---
title: ZeroFormatter Serializer Usage
summary: Using the ZeroFormatter serializer in an endpoint
component: ZeroFormatter
reviewed: 2018-09-28
related:
 - nservicebus/serialization
 - nservicebus/serialization/zeroformatter
---


## Configuring NServiceBus to use ZeroFormatter

snippet: config


## The message class

snippet: message


## Sending a message

The message is decorated with [ZeroFormatter attributes](https://github.com/neuecc/ZeroFormatter/#define-object-rules).

snippet: messagesend