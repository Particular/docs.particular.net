---
title: Jil Serializer
summary: Using the Jil serializer in an endpoint.
component: Jil
reviewed: 2016-10-31
related:
 - nservicebus/serialization
 - nservicebus/serialization/jil
---


## Configuring to use Jil

snippet:config


## Diagnostic Mutator

A helper that will Write out the contents of any incoming message.

snippet:mutator


## The message send

snippet:message
 

## The Output

```json
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