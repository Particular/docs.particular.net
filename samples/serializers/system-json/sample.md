---
title: System.Text.Json Serializer sample
summary: NServiceBus sample that shows how to use the builtin System.Text.Json serializer in an endpoint
reviewed: 2025-05-19
component: SystemJson
related:
 - nservicebus/serialization/system-json
 - nservicebus/serialization
---

This sample uses the [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json) serializer built in to NServiceBus to serialize message payloads.

## Configuring to use NServiceBus.Newtonsoft.Json

snippet: config

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
