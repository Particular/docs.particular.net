---
title: Jil Serializer
summary: Using the Jil serializer in an endpoint.
component: Jil
reviewed: 2016-03-21
related:
- nservicebus/serialization
---

## NServiceBus.Jil

This sample uses the community run serializer [NServiceBus.Jil](https://github.com/SimonCropp/NServiceBus.Jil).


## Configuring to use Jil

snippet:config


## Diagnostic Mutator

A helper that will Write out the contents of any incoming message.

snippet:mutator


## The message send

snippet:message
  

## The Output

```
{
 "Date": "\/Date(1442274949826)\/",
 "CustomerId": 12,
 "OrderId": 9,
 "OrderItems": [{
  "Quantity": 2,
  "ItemId": 6
 }, {
  "Quantity": 4,
  "ItemId": 5
 }]
}
```