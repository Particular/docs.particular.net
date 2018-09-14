---
title: Wire Serializer Usage
summary: Demonstrates the use of the Wire serializer in an endpoint.
component: Wire
reviewed: 2018-09-14
related:
 - nservicebus/serialization
 - nservicebus/serialization/wire
---


## Configuring to use Wire

snippet: config


## Sending a message

snippet: message


## The output

Since Wire is a binary format, the message body is not human-readable.

![](wirebinary.png)