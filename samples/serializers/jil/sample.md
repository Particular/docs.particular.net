---
title: Jil Serializer
summary: How to use the Jil serializer in an endpoint.
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
