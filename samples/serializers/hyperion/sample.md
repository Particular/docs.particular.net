---
title: Hyperion Serializer Usage
summary: Using the Hyperion serializer in an endpoint.
component: Hyperion
reviewed: 2017-06-01
related:
 - nservicebus/serialization
 - nservicebus/serialization/hyperion
---


## Configuring to use Hyperion

snippet: config


## The message send

snippet: message


## The Output

Since Hyperion is a binary format there is not much human readable in the message body.

![](binary.png)