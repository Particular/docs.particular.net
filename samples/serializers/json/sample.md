---
title: JSON Serializer
summary: Using the core JSON serializer.
reviewed: 2016-03-21
component: Json
related:
- nservicebus/serialization
- nservicebus/serialization/json
---


include: json-deprecation

This samples uses the JSON serializer in the core.


## Configuring to use JSON

snippet: config


## Diagnostic Mutator

A helper that will Write out the contents of any incoming message.

snippet: mutator


## The message send

snippet: message
 

## The Output

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