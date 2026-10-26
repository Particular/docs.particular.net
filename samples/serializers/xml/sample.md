---
title: XML Serializer sample
summary: NServiceBus sample that shows how to use the built-in NServiceBus XML serializer
reviewed: 2026-10-26
component: Xml
related:
- nservicebus/serialization
- nservicebus/serialization/xml
---

This sample demonstrates the built-in NServiceBus XML serializer.

## Configuring to use XML

snippet: config

## Diagnostic mutator

A helper that will log the contents of any incoming message:

snippet: mutator

## Sending the message

snippet: message

## Output

The serializer writes the body as a single line, so the value below is exactly what the mutator logs. The `xmlns` value is derived from the message's CLR namespace as `http://tempuri.net/{namespace}`; since the sample's message types are declared in the global namespace, it ends with a trailing slash.

```xml
<?xml version="1.0"?><CreateOrder xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://tempuri.net/"><OrderId>9</OrderId><Date>2026-10-26T08:13:11.7341761+01:00</Date><CustomerId>12</CustomerId><OrderItems><OrderItem><ItemId>6</ItemId><Quantity>2</Quantity></OrderItem><OrderItem><ItemId>5</ItemId><Quantity>4</Quantity></OrderItem></OrderItems></CreateOrder>
```
