---
title: XML Serializer
summary: Using the default XML serializer.
reviewed: 2016-03-21
component: Core
related:
- nservicebus/serialization
- nservicebus/serialization/xml
---

This sample uses the default XML serializer in the core.


## Configuring to use XML

snippet:config


## Diagnostic Mutator

A helper that will Write out the contents of any incoming message.

snippet:mutator


## The message send

snippet:message
 

## The Output

```xml
<?xml version="1.0" ?>
<CreateOrder xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://tempuri.net/XmlSample">
	<OrderId>9</OrderId>
	<Date>2015-09-15T13:33:28.7186624+10:00</Date>
	<CustomerId>12</CustomerId>
	<OrderItems>
		<OrderItem>
			<ItemId>6</ItemId>
			<Quantity>2</Quantity>
		</OrderItem>
		<OrderItem>
			<ItemId>5</ItemId>
			<Quantity>4</Quantity>
		</OrderItem>
	</OrderItems>
</CreateOrder>
```