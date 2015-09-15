---
title: JSON Serializer
summary: How to use the core JSON serializer.
related:
- nservicebus/serialization
- nservicebus/serialization/json
---

This samples uses the JSON serializer in the core.

## Configuring to use JSON 

<!-- import config -->


## Diagnostic Mutator

A helper that will Write out the contents of any incoming message. 

<!-- import mutator -->


## The message send

<!-- import message -->
   

## The Output

```
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