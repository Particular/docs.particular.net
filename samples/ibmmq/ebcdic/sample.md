---
title: IBM MQ EBCDIC interoperability
summary: Receiving fixed-length EBCDIC-encoded messages from a legacy mainframe system via IBM MQ
reviewed: 2026-03-20
component: Core
related:
- transports/ibmmq
- transports/ibmmq/native-integration
- nservicebus/pipeline/message-mutators
- nservicebus/messaging/headers
- nservicebus/serialization/system-json
---

This sample demonstrates how to receive fixed-length [EBCDIC](https://en.wikipedia.org/wiki/EBCDIC)-encoded messages sent by a legacy mainframe system over IBM MQ, and process them in a modern NServiceBus endpoint.

The sample includes:

- A **LegacySender** console application that simulates a mainframe producing EBCDIC-encoded fixed-length records directly to IBM MQ using the native `IBM.WMQ` API
- A **Receiver** NServiceBus endpoint that receives those raw messages, decodes them, and processes them as standard NServiceBus [commands](/nservicebus/messaging/messages-events-commands.md)

## How it works

Mainframe systems often communicate using EBCDIC encoding and fixed-length binary record layouts rather than JSON or XML. Such messages carry no [NServiceBus headers](/nservicebus/messaging/headers.md) and cannot be deserialized by the endpoint without prior transformation.

The NServiceBus [message mutator](/nservicebus/pipeline/message-mutators.md) pipeline extension point intercepts raw incoming messages before they reach the [message handler pipeline](/nservicebus/pipeline/). An incoming transport message mutator (`IMutateIncomingTransportMessages`) can inspect the raw bytes, decode them, and replace the message body and headers - making the transformed message indistinguishable from one sent natively by NServiceBus.

## Running the sample

The sample requires a running IBM MQ instance. A Docker Compose configuration is recommended. 

## Code walk-through

### Legacy sender

The `LegacySender` project simulates a mainframe producing a 70-byte fixed-length EBCDIC record and putting it directly on the `DEV.RECEIVER` queue using the native `IBM.WMQ` API. The message carries no [MQRFH2 headers](/transports/ibmmq/native-integration.md#message-structure) and no NServiceBus metadata.

The record layout is:

| Bytes | Field    | Encoding                                       |
|-------|----------|------------------------------------------------|
| 0-35  | OrderId  | EBCDIC code page 500, 36 chars                 |
| 36-65 | Product  | EBCDIC code page 500, space-padded to 30 chars |
| 66-69 | Quantity | Big-endian `Int32`                             |

The `MQMessage.CharacterSet` is set to `500` (EBCDIC) and `MQMessage.Format` is set to `MQFMT_NONE`, indicating raw binary content with no broker-managed structured headers.

snippet: CreateEBCDICMessage

### Registering the mutator

`EbcdicMutator` is registered directly in endpoint configuration:

snippet: FixedLengthEBCDICToJsonMutatorRegistration

### Decoding the EBCDIC message

`EbcdicMutator` implements `IMutateIncomingTransportMessages`. It is called for every incoming message before the message is dispatched to a [message handler](/nservicebus/handlers/).

snippet: EbcdicMutator

The mutator:

1. Returns early for records that are not exactly 70 bytes - allowing the message to proceed unchanged.
2. Decodes the three fixed-length fields from EBCDIC bytes into .NET types using `Encoding.GetEncoding(500)` and `BinaryPrimitives.ReadInt32BigEndian`.
3. Replaces `context.Body` with a JSON-encoded body compatible with the [`SystemJsonSerializer`](/nservicebus/serialization/system-json.md) configured on the endpoint.
4. Adds [headers](/nservicebus/messaging/headers.md) to `context.Headers` that provide all the NServiceBus routing metadata the pipeline requires: message type, content type, intent, reply-to address, and originating endpoint.

### Message handler

`PlaceOrderHandler` processes the decoded [`PlaceOrder`](/nservicebus/messaging/messages-events-commands.md) command using standard NServiceBus APIs - it has no awareness of the EBCDIC origin of the message:

snippet: PlaceOrderHandler
