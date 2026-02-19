---
title: Native integration
summary: How to integrate NServiceBus endpoints with native IBM MQ applications
reviewed: 2026-02-19
component: IbmMq
---

The IBM MQ transport stores NServiceBus messages using standard IBM MQ message structures, making it possible to send and receive messages between NServiceBus endpoints and native IBM MQ applications.

## Message structure

NServiceBus messages on IBM MQ consist of two parts:

### MQMD (Message Descriptor)

The MQMD is the standard IBM MQ message header. The transport maps the following fields:

|MQMD field|NServiceBus usage|
|:---|---|
|MessageId|24-byte binary; set from the `NServiceBus.MessageId` header as a GUID padded to 24 bytes|
|CorrelationId|24-byte binary; set from the `NServiceBus.CorrelationId` header|
|MessageType|Always `MQMT_DATAGRAM` (8)|
|Persistence|`MQPER_PERSISTENT` by default; `MQPER_NOT_PERSISTENT` if `NonDurableMessage` header is set|
|Expiry|Set from `DiscardIfNotReceivedBefore` in tenths of seconds; `MQEI_UNLIMITED` if not specified|
|CharacterSet|UTF-8 (CCSID 1208) by default|
|ReplyToQueueName|Set from the `NServiceBus.ReplyToAddress` header|

### MQRFH2 properties

All NServiceBus headers are stored as MQRFH2 custom properties on the message. Property names are escaped to comply with IBM MQ naming rules:

- Underscores are doubled: `_` becomes `__`
- Non-alphanumeric characters are encoded as `_xHHHH` where `HHHH` is the Unicode code point

For example, the header `NServiceBus.MessageId` is stored as the property `NServiceBus_x002EMessageId`.

Two additional manifest properties are included:

|Property|Purpose|
|:---|---|
|`nsbhdrs`|Comma-separated list of all escaped header names|
|`nsbempty`|Comma-separated list of escaped header names with empty values|

> [!NOTE]
> IBM MQ silently discards string properties with empty values. The manifest properties work around this limitation by tracking which headers exist and which have empty values.

## Sending messages from native applications

To send a message to an NServiceBus endpoint from a native IBM MQ application:

1. Create an `MQMessage` targeting the endpoint's input queue.
2. Set the message body to a serialized representation of the message (e.g., JSON or XML).
3. Set MQRFH2 properties for the required NServiceBus headers.

The minimum required headers are:

|Header (escaped property name)|Description|
|:---|---|
|`NServiceBus_x002EEnclosedMessageTypes`|The fully qualified .NET type name of the message|
|`NServiceBus_x002EContentType`|The MIME type of the body serialization (e.g., `application/json`)|
|`NServiceBus_x002EMessageId`|A unique identifier for the message (typically a GUID)|

> [!WARNING]
> If the `NServiceBus.EnclosedMessageTypes` header is missing, the endpoint will not be able to deserialize and route the message.

## Receiving messages in native applications

Native applications can consume messages from queues that NServiceBus publishes or sends to. The application should:

1. Read the message body bytes from the `MQMessage`.
2. Read MQRFH2 properties to access NServiceBus headers. Use the `nsbhdrs` manifest to enumerate all headers, and `nsbempty` to identify headers with empty values.
3. Unescape property names by replacing `__` with `_` and `_xHHHH` with the corresponding character.

## Interoperability considerations

- NServiceBus always sets `MessageType` to `MQMT_DATAGRAM`. Native applications that rely on `MQMT_REQUEST`/`MQMT_REPLY` patterns should use a separate set of queues or translate at an integration boundary.
- The body encoding is determined by the NServiceBus serializer configuration, typically JSON. Native applications must use the same serialization format.
- Message expiry (time-to-be-received) is mapped to the MQMD `Expiry` field in tenths of seconds.
