---
title: Utf8Json Serializer Usage
summary: Using the Utf8Json serializer in an endpoint.
component: Utf8Json
reviewed: 2017-09-29
related:
 - nservicebus/serialization
 - nservicebus/serialization/utf8json
---


## Configuring to use Utf8Json

snippet: config


## Diagnostic Mutator

A helper that will write out the contents of any incoming message.

snippet: mutator


## The message send

snippet: message
 

## The Output

```json
{
   "OrderId":9,
   "Date":"2017-09-29T22:38:06.9444190+10:00",
   "CustomerId":12,
   "OrderItems":[  
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