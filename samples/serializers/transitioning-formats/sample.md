---
title: Transition serialization formats
reviewed: 2016-10-18
component: Core
related:
- nservicebus/serialization
---

This sample illustrates an approach for introducing a breaking change to the [message serialization](/nservicebus/serialization/) format in way that requires no endpoint downtime and no manipulation of message bodies.

NOTE: This sample uses 4 "Phase" Endpoint Projects to illustrate the iterations of a single endpoint in one solution.

The [External Json.NET Serializer](/nservicebus/serialization/newtonsoft.md) is used in this sample, but the Phased upgrade approach is applicable to changing the format any serializer or even moving between different serializers.


## Message Contracts

snippet: messages


## Message Compatibility

To highlight compatibility between iterations of the endpoint, each Phase sends a message to itself, the upper phase, and the lower phase.

For example Phase3 sends to itself, Phase2, and Phase4.

snippet: send-to-both


## Serialization format change

For demonstration purposes this sample uses a hypothetical scenario where the Json serialization format need to change the approach for serializing dictionaries. From using contents for the key and value to using an explicitly named key and value approach.


### Json using standard approach

```json
{
  "OrderId": 9,
  "OrderItems": {
    "3": {
      "Quantity": 2
    },
    "8": {
      "Quantity": 7
    }
  }
}
```


### JSON using key-value approach

```json
{
  "OrderId": 9,
  "OrderItems": [
    {
      "Key": 3,
      "Value": {
        "Quantity": 2
      }
    },
    {
      "Key": 8,
      "Value": {
        "Quantity": 7
      }
    }
  ]
}
```


### Implementing the change

This is implemented using [NewtonSoft ContractResolver](http://www.newtonsoft.com/json/help/html/contractresolver.htm) and changing the using an array contract for the dictionary instead of the default.

snippet: ExtendedResolver


## Diagnostic helpers

To help visualize the serialization changes there are two [Behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md) that write the contents of each incoming and outgoing message.

snippet: IncomingWriter

snippet: OutgoingWriter

Both Behaviors are registered at configuration time:

snippet: AddMessageBodyWriter

snippet: writer-registration


## Phases

Note that in production system each of the phases would need to be applied to every endpoint that needs communicate with the new serialization format. So each endpoint can be at-most one phase out of sync with any other endpoint it needs to communicate with.


### Phase 1

Endpoint is running:

 * Version 1 serialization. 

This is the initial state  where all endpoints are using the standard `JsonSerializerSettings` registered with the `ContentTypeKey` of `jsonv1`. All endpoints are sending and receiving V1 messages.

snippet: Phase1


### Phase 2

Endpoint is running:

 * Version 1 serialization.
 * Version 2 deserialization.

The new `JsonSerializerSettings` registered as a deserializer with the `ContentTypeKey` of `jsonv2`. This makes all endpoint capable of receiving V2 messages while still sending V1 messages.

snippet: Phase2


### Phase 3

Endpoint is running:

 * Version 2 serialization
 * Version 1 deserialization

The serializer and deserializer are switched. So all endpoints can still receive V1 messages but will be sending V2 messages.

snippet: Phase3


### Phase 4

Endpoint is running:

 * Version 2 serialization

All endpoints are now sending and receiving V2 messages.

snippet: Phase4


## Messages in transit

It is important to consider both [Discard Old Messages](/nservicebus/messaging/discard-old-messages.md) and how the [Error Queue](/nservicebus/recoverability/configure-error-handling.md) is handled. For example the following time-line could be problematic 

 * A message makes use of the old serialization format.
 * The message a long Time-To-Be-Received (TTBR).
 * The message fails processing and is forwarded to the error queue.
 * The above change in serialization format is performed.
 * A retry of the message is attempted.

Given the type of serialization change that has occurred, the structure of the message contract, and the serializer being used the resulting behavior is non-deterministic. The serializer may throw and exception for the old format or it may swallow the error and proceed with missing data.

So either

 * Delay the upgrading of each Phase so the overlapping time is greater than the maximum TTBR of message contracts. Or;
 * During the deployment of each Phase verify that messages in the error queue will not exhibit the above problem.
