---
title: XML Serializer
summary: Using the default XML serializer
reviewed: 2018-02-23
component: Xml
related:
- nservicebus/serialization
- nservicebus/serialization/xml
---

This sample uses the default XML serializer in the core.


## Configuring to use XML

snippet: config


## Diagnostic mutator

A helper that will log the contents of any incoming message.

snippet: mutator


## Sending the message

snippet: message


## Output

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
