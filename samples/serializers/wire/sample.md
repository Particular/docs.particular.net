---
title: Wire Serializer
summary: Using the Wire serializer in an endpoint.
component: Wire
reviewed: 2016-03-21
related:
- nservicebus/serialization
---

## NServiceBus.Wire

This sample uses the community run serializer [NServiceBus.Wire](https://github.com/hmemcpy/NServiceBus.Wire) to serialize messages with the [Wire](https://github.com/rogeralsing/Wire) binary format.


## Configuring to use Wire

snippet:config


## The message send

snippet:message


## The Output

Since Wire is a binary format there is not much human readable in the message body.

![](wirebinary.png)