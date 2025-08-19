---
title: Consuming MassTransit messages with NServiceBus
summary: Use the NServiceBus pipeline to consume messages sent by MassTransit.
reviewed: 2024-09-22
component: Core
related:
 - nservicebus/pipeline
 - nservicebus/pipeline/manipulate-with-behaviors
---

This sample shows how existing systems built with MassTransit can be integrated with new NServiceBus systems by using a [message pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) in the NServiceBus endpoints to translate incoming MassTransit messages into a shape that can be successfully processed. This technique could also be used to migrate those systems endpoint-by-endpoint, if that is desired.

downloadbutton

## Prerequisites

This sample requires a local instance of RabbitMQ.

The easiest way to do this is to run RabbitMQ in Docker by running the following command

```shell
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

## Message structure comparison

When using a message structured like this:

snippet: MassTransitEvent

MassTransit translates the message in RabbitMQ as follows:

**Properties**

* **message_id**: 5fdc0000-426e-001c-fcf9-08d9a30339e8
* **delivery_mode**: 2
* **headers**:
  * **Content**-Type: application/vnd.masstransit+json
  * **publishId**: 9
* **content_type**: application/vnd.masstransit+json

**Payload**

```json
{
  "messageId": "5fdc0000-426e-001c-fcf9-08d9a30339e8",
  "conversationId": "5fdc0000-426e-001c-fda0-08d9a30339e8",
  "sourceAddress": "rabbitmq://hostos/MACHINENAME_MTEndpoint_bus_m9qyyynnpayb3rk1bdc4gy3wyc?temporary=true",
  "destinationAddress": "rabbitmq://hostos/Messages.Events:MassTransitEvent",
  "messageType": [
    "urn:message:Messages.Events:MassTransitEvent"
  ],
  "message": {
    "text": "The time is 11/8/2021 4:00:50 PM -06:00"
  },
  "sentTime": "2021-11-08T22:00:50.1435641Z",
  "headers": {},
  "host": {
    "machineName": "MACHINENAME",
    "processName": "MTEndpoint",
    "processId": 13892,
    "assembly": "MTEndpoint",
    "assemblyVersion": "1.0.0.0",
    "frameworkVersion": "5.0.11",
    "massTransitVersion": "7.2.4.0",
    "operatingSystemVersion": "Microsoft Windows NT 10.0.19043.0"
  }
}
```

By contrast, NServiceBus structures a message on RabbitMQ differently, reserving the payload entirely for the message body, and putting the metadata into RabbitMQ properties. If NServiceBus had published the event, it would look like this:

**Properties**

* **$.diagnostics.originating.hostid**:	df9d8d0d7e92df4f82efac8bab50d815
* **NServiceBus.ContentType**:	application/json
* **NServiceBus.ConversationId**:	5fdc0000-426e-001c-5de8-08d9a3033b36
* **NServiceBus.CorrelationId**:	5fdc0000-426e-001c-5d52-08d9a3033b36
* **NServiceBus.EnclosedMessageTypes**:	Messages.Events.MassTransitEvent, Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
* **NServiceBus.MessageId**:	bcf527ec-1fc6-4228-a753-adda016cd7fb
* **NServiceBus.MessageIntent**:	Publish
* **NServiceBus.OriginatingEndpoint**:	NServiceBusSubscriber
* **NServiceBus.OriginatingMachine**:	MACHINENAME
* **NServiceBus.RelatedTo**:	b26e5ffd-fa79-4eb1-b3b9-adda016cd7d9
* **NServiceBus.ReplyToAddress**:	NServiceBusSubscriber
* **NServiceBus.TimeSent**:	2021-11-08 22:08:21:312401 Z
* **NServiceBus.Transport.RabbitMQ.ConfirmationId**:	86
* **NServiceBus.Version**:	7.5.0

**Payload**

```json
{"Text":"The time is 11/8/2021 4:00:52 PM -06:00"}
```

In order for a MassTransit message to be understood, an NServiceBus [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) can be used to convert the format of the MassTransit RabbitMQ message to a shape that looks like an NServiceBus message.

## MassTransit publisher

In the sample, the MassTransit publisher is largely unchanged from the example in the [MassTransit Getting Started guide](https://masstransit-project.com/getting-started/). Inside the Worker background service, a loop publishes an event every second until the process shuts down:

snippet: MassTransitPublish

Additionally, the MassTransit endpoint contains a message consumer for the event, subscribing to the event it is publishing:

snippet: MassTransitConsumer

Because the event is published in RabbitMQ, NServiceBus can also subscribe to it.

> [!NOTE]
> This sample shows how to use an NServiceBus behavior to ingest messages from a MassTransit system. Alternatively, the [MassTransit.Interop.NServiceBus](https://nuget.org/packages/MassTransit.Interop.NServiceBus/) package may be used, as described in [the MassTransit docs](https://masstransit.io/documentation/configuration/integrations/nsb).

## NServiceBus subscriber

### Basic setup

The configuration for the [RabbitMQ transport](/transports/rabbitmq/) is standard:

snippet: Transport

Because MassTransit uses JSON to serialize its messages, NServiceBus needs to use the [Json.NET serializer](/nservicebus/serialization/newtonsoft.md).

snippet: Serializer

Although NServiceBus by default uses the marker interfaces `ICommand` and `IEvent` to identify messages, looking back at the event class, the `IEvent` interface is not used:

snippet: MassTransitEvent

It is also not recommended to take a message assembly containing messages published by MassTransit and force it to take a dependency on NServiceBus. Instead, [message conventions](/nservicebus/messaging/conventions.md) can be defined to identify messages by other means, such as by namespace:

snippet: Conventions

### MassTransit ingest behavior

With NServiceBus set up to use RabbitMQ, serialize with JSON, and recognize MassTransit message classes as messages, a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) manipulates the RabbitMQ message before it is deserialized, transforming it into the shape of an NServiceBus message.

This code snippet is lengthy, and is designed to translate many optional data elements into the NServiceBus message headers by manipulating JSON.NET primitives. Many solutions will not require this level of detail, because NServiceBus only requires the **NServiceBus.MessageId** and **NServiceBus.EnclosedMessageTypes** headers in order to successfully process a message.

snippet: Behavior

Registering the behavior to be run as part of the message processing pipeline requires one additional line of configuration:

snippet: RegisterBehavior

For completeness, here is the NServiceBus message handler, which is very simple and does not need to know that the message it is processing originally came from MassTransit.

snippet: NSBMessageHandler

## Output

When running the sample, both endpoints receive and process each event published by MassTransit.

<!-- NOTE: A space precedes each log line so that "info" doesn't turn into alert boxes -->

**MassTransit Endpoint**

```shell
 info: MassTransit[0]
       Configured endpoint Message, Consumer: MTEndpoint.MessageConsumer
 info: Microsoft.Hosting.Lifetime[0]
       Application started. Press Ctrl+C to shut down.
 info: Microsoft.Hosting.Lifetime[0]
       Hosting environment: Development
 info: Microsoft.Hosting.Lifetime[0]
       Content root path: C:\code\masstransit-messages\MTEndpoint
 info: MassTransit[0]
       Bus started: rabbitmq://hostos/
 info: MTEndpoint.MessageConsumer[0]
       Received Text: The time is 11/8/2021 4:39:33 PM -06:00
 info: MTEndpoint.MessageConsumer[0]
       Received Text: The time is 11/8/2021 4:39:34 PM -06:00
 info: MTEndpoint.MessageConsumer[0]
       Received Text: The time is 11/8/2021 4:39:35 PM -06:00
```

**NServiceBus Subscriber**

```log
 info: Microsoft.Hosting.Lifetime[0]
       Application started. Press Ctrl+C to shut down.
 info: Microsoft.Hosting.Lifetime[0]
       Hosting environment: Development
 info: Microsoft.Hosting.Lifetime[0]
       Content root path: C:\code\masstransit-messages\NServiceBusSubscriber
 info: NServiceBusSubscriber.MassTransitEventHandler[0]
       Received Text: The time is 11/8/2021 4:39:33 PM -06:00
 info: NServiceBusSubscriber.MassTransitEventHandler[0]
       Received Text: The time is 11/8/2021 4:39:34 PM -06:00
 info: NServiceBusSubscriber.MassTransitEventHandler[0]
       Received Text: The time is 11/8/2021 4:39:35 PM -06:00
```
