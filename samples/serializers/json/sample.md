---
title: JSON Serializer sample
summary: NServiceBus sample that shows how to usr the core JSON serializer
reviewed: 2022-03-25
component: Json
related:
- nservicebus/serialization
- nservicebus/serialization/json
---

include: json-deprecation

This samples uses the JSON serializer in the core.

## Configuring to use JSON

snippet: config

## Diagnostic mutator

A helper that will log the contents of any incoming message:

snippet: mutator

## Sending the message

snippet: message

## Output

```json
{
 "OrderId":9,
 "Date":"2015-09-15T13:42:00.2776276+10:00",
 "CustomerId":12,
 "OrderItems":
 [
  {
   "ItemId":6,
   "Quantity":2
  },
  {
   "ItemId":5,
   "Quantity":4
  }
 ]
}
```
