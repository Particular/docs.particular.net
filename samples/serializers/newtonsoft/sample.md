---
title: Newtonsoft Json Serializer
summary: Use the Newtonsoft Json serializer in an endpoint.
reviewed: 2016-03-21
component: NewtonSoft
related:
 - nservicebus/serialization
 - samples/serializers/newtonsoft-bson
 - samples/encryption/newtonsoft-json-encryption
redirects:
 - samples/serializers/json-external
---


This sample uses the Newtonsoft serializer [NServiceBus.Newtonsoft.Json](https://github.com/Particular/NServiceBus.Newtonsoft.Json) to provide full access to the [Newtonsoft Json.net](http://www.newtonsoft.com/json) API.


## Configuring to use NServiceBus.Newtonsoft.Json

snippet: config


## Diagnostic Mutator

A helper that will Write out the contents of any incoming message.

snippet: mutator

Register the mutator.

snippet: registermutator


## The message send

snippet: message
 

## The Output

```json
?{
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