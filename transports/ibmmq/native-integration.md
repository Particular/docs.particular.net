---
title: Native integration
summary: How to integrate NServiceBus endpoints with native IBM MQ applications
reviewed: 2026-02-19
component: IbmMq
---

The IBM MQ transport uses standard IBM MQ message structures, making it possible to exchange messages between NServiceBus endpoints and native IBM MQ applications.

## Message structure

NServiceBus messages on IBM MQ consist of a standard message descriptor (MQMD) and custom properties stored in MQRFH2 headers.

### MQMD fields

The transport maps NServiceBus concepts to the following MQMD fields:

|MQMD field|Usage|
|:---|---|
|MessageId|Set from the `NServiceBus.MessageId` header|
|CorrelationId|Set from the `NServiceBus.CorrelationId` header|
|MessageType|Always `MQMT_DATAGRAM`|
|Persistence|Persistent by default; non-persistent if `NonDurableMessage` header is set|
|Expiry|Set from the time-to-be-received setting; unlimited if not specified|
|ReplyToQueueName|Set from the `NServiceBus.ReplyToAddress` header|

### NServiceBus headers

All NServiceBus headers are stored as MQRFH2 message properties. Property names are escaped because IBM MQ only allows alphanumeric characters and underscores in property names. The escaping rules are:

- Underscores are doubled: `_` becomes `__`
- Other special characters are encoded as `_xHHHH` (e.g., `.` becomes `_x002E`)

> [!NOTE]
> IBM MQ silently discards string properties with empty values. The transport includes manifest properties (`nsbhdrs` and `nsbempty`) to track all header names and preserve empty values.

## Sending messages from native applications

To send a message to an NServiceBus endpoint from a native IBM MQ application:

1. Put a message on the endpoint's input queue.
2. Set the message body to a serialized representation (e.g., JSON).
3. Set the following MQRFH2 properties as a minimum:

|Property name|Description|
|:---|---|
|`NServiceBus_x002EEnclosedMessageTypes`|The fully qualified .NET type name of the message|
|`NServiceBus_x002EContentType`|The MIME type of the body (e.g., `application/json`)|
|`NServiceBus_x002EMessageId`|A unique identifier (typically a GUID)|

> [!WARNING]
> If the `EnclosedMessageTypes` header is missing, the endpoint will not be able to deserialize and route the message.

## Receiving messages in native applications

Native applications can consume messages from queues that NServiceBus publishes or sends to. The message body contains the serialized payload, and NServiceBus headers are available as MQRFH2 properties.

To read headers, use the `nsbhdrs` manifest property to enumerate all header names, and unescape property names by replacing `__` with `_` and `_xHHHH` with the corresponding character.
