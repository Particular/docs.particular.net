---
title: Newtonsoft JSON Serializer sample
summary: NServiceBus sample that shows how to use the Newtonsoft JSON serializer in an endpoint
reviewed: 2025-11-03
component: Newtonsoft
related:
 - nservicebus/serialization
 - samples/serializers/newtonsoft-bson
redirects:
 - samples/serializers/json-external
---


This sample uses the Newtonsoft serializer [NServiceBus.Newtonsoft.Json](https://github.com/Particular/NServiceBus.Newtonsoft.Json) to provide full access to the [Newtonsoft Json.net](https://www.newtonsoft.com/json) API.

## Configuring to use NServiceBus.Newtonsoft.Json

snippet: config

## Diagnostic mutator

A helper that will log the contents of any incoming message:

snippet: mutator

Register the mutator:

snippet: registermutator

## Sending the message

snippet: message

## Output

```json
{
  "OrderId": 9,
  "Date": "2015-09-15T10:23:44.9367871+10:00",
  "CustomerId": 12,
  "OrderItems": [
    {
      "ItemId": 6,
      "Quantity": 2
    },
    {
      "ItemId": 5,
      "Quantity": 4
    }
  ]
}
```
