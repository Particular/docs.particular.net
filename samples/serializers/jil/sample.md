---
title: Jil Serializer Usage
summary: Demonstrates the use of the Jil serializer in an endpoint.
component: Jil
reviewed: 2018-09-14
related:
 - nservicebus/serialization
 - nservicebus/serialization/jil
---


## Configuring to use Jil

snippet: config


## Diagnostic mutator

This sample uses a mutator to write out the contents of all incoming messages:

snippet: mutator


## Sending a message

snippet: message
 

## The output

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