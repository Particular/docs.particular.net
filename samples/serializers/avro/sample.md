---
title: Avro Serializer sample
summary: NServiceBus sample that shows how to use the Avro serializer in an endpoint
reviewed: 2025-08-12
component: Core
related:
 - nservicebus/serialization
---

> [!NOTE]
> This sample showcases how serialization using Avro can work, but it is not a supported serializer and some use cases may not work. See our [serializer documentation for a full list of supported serializers](/nservicebus/serialization/#supported-serializers).

This sample uses [Apache.Avro data serialization system](https://www.nuget.org/packages/apache.avro) to serialize and deserialize the messages.

## Limitations

This sample has the following limitations:

- It does not support [message types defined using C# interfaces](/nservicebus/messaging/messages-as-interfaces.md)
- It is not able to infer the message type from the payload like some other serializers; therefore, the [`NServiceBus.EnclosedMessageTypes` header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) must be present on all messages.

## Configuration

Configure the endpoint to use the Avro serializer as follows:

snippet: config

## Schema registry

The sample expects the message schema to be present as an embedded resource in the same folder where the message type resides. The schemas are read on startup and cached into a SchemaRegistry.
This choice was made to simplify the use of this sample. In production, it's recommended to use a central registry like:

- [Event Hubs Schema Registry](https://learn.microsoft.com/en-us/azure/event-hubs/schema-registry-concepts)
- [Confluent Schema Registry](https://docs.confluent.io/platform/current/schema-registry/serdes-develop/index.html)
- [AWS Glue Schema Registry](https://docs.aws.amazon.com/glue/latest/dg/schema-registry.html)
- [Apicurio Registry](https://www.apicur.io/registry/)
- [Redpanda Schema Registry](https://docs.redpanda.com/current/manage/schema-reg)

> [!NOTE]
> When a schema is not found, a `MessageDeserializationException` will be thrown, which will cause the message to [be moved to the configured error queue](/nservicebus/recoverability/#fault-handling) without retries.

## Code

The serializer is implemented as a [custom serializer](/nservicebus/serialization/custom-serializer.md) by creating a `SerializerDefinition`:

snippet: serializer-definition

and also implementing the message serializer interface:

snippet: serializer-implementation

## Sending the message

Prepare and send an order message with sample data:

snippet: message

## Output

The serialized message output appears as follows:

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
