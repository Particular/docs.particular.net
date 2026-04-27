---
title: Native integration
summary: How to integrate NServiceBus endpoints with native IBM MQ applications
reviewed: 2026-03-23
component: IBMMQ
---

The IBM MQ transport uses standard IBM MQ message structures, making it possible to exchange messages between NServiceBus endpoints and native IBM MQ applications.

## Message structure

NServiceBus messages on IBM MQ consist of a standard message descriptor (MQMD) and custom properties stored in MQRFH2 headers.

### MQMD fields

The transport maps NServiceBus concepts to the following MQMD fields on send:

|MQMD field|Usage|
|:---|---|
|MessageId|Set from the `NServiceBus.MessageId` header. Accepts a GUID or a hex string up to 24 bytes.|
|CorrelationId|Set from the `NServiceBus.CorrelationId` header. Accepts a GUID or a hex string up to 24 bytes.|
|MessageType|Always `MQMT_DATAGRAM`|
|Persistence|Persistent by default; non-persistent if `NonDurableMessage` header is set|
|Expiry|Set from the time-to-be-received setting; unlimited if not specified|
|ReplyToQueueName|Set from the `NServiceBus.ReplyToAddress` header|

### NServiceBus headers

All NServiceBus headers are stored as MQRFH2 message properties. Property names are escaped because IBM MQ only allows alphanumeric characters and underscores in property names. The escaping rules are:

- Underscores are doubled: `_` becomes `__`
- Other special characters are encoded as `_xHHHH` (e.g., `.` becomes `_x002E`)

> [!NOTE]
> IBM MQ silently discards string properties with empty values. To preserve all header names and maintain empty values, the transport uses manifest properties: `nsbhdrs` (lists all header names) and `nsbempty` (tracks empty-valued headers). The header fields with non-compliant names will now be displayed in the IBMMQ Console.

## Receiving from non-NServiceBus senders

When a message arrives from a native IBM MQ application that does not set MQRFH2 properties, the transport promotes the following MQMD fields to NServiceBus headers. Promotion only occurs if the corresponding NServiceBus header is not already set. This ensures NServiceBus-originated messages are not overwritten.

|MQMD field|NServiceBus header|Notes|
|:---|---|---|
|`MessageId`|`NServiceBus.MessageId`|Encoded as an uppercase hex string|
|`CorrelationId`|`NServiceBus.CorrelationId`|Encoded as an uppercase hex string|
|`ReplyToQueueName`|`NServiceBus.ReplyToAddress`|Trimmed of whitespace; ignored if empty|
|`Persistence`|`NServiceBus.NonDurableMessage`|Set to `True` when persistence is `NOT_PERSISTENT`|
|`Expiry`|`NServiceBus.TimeToBeReceived`|Converted from tenths of seconds; ignored when unlimited|

This enables native senders to rely on standard MQMD fields for identity and routing without requiring knowledge of the NServiceBus header format.

## Sending messages from native applications

To send a message to an NServiceBus endpoint from a native IBM MQ application:

1. Put a message on the endpoint's input queue.
2. Set the message body to your serialized content (e.g., JSON). NServiceBus will deserialize it based on the `ContentType` property you set.
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

To read NServiceBus headers from received messages:

1. Use the `nsbhdrs` manifest property to enumerate all header names.
2. For each header name, unescape the property name by:
   - Replacing `__` with `_`
   - Replacing `_xHHHH` with the corresponding character (e.g., `_x002E` becomes `.`)
