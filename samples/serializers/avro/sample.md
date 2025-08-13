---
title: Avro Serializer sample
summary: NServiceBus sample that shows how to use the Avro serializer in an endpoint
reviewed: 2025-08-12
component: Core
related:
 - nservicebus/serialization
---


This sample uses the [Apache.Avro serializer](https://www.nuget.org/packages/apache.avro) to serialize and deserialize the messages.

## Limitations

- The serializer is not able to infer the message type from the payload like some other serializers, so the [`NServiceBus.EnclosedMessageTypes` header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) must be present on all messages.

## Configuration

snippet: config

## Schema registry

The sample store the message schema as an embedded resource for ease of use, in production its recommended to use a central registry like:

- <https://docs.confluent.io/platform/current/schema-registry/serdes-develop/index.html>
- <https://www.redpanda.com/blog/schema-registry-kafka-streaming#:~:text=In%20the%20context%20of%20Kafka,popular%20choice%20for%20data%20serialization>.
- <https://docs.confluent.io/platform/current/schema-registry/fundamentals/serdes-develop/index.html>

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
