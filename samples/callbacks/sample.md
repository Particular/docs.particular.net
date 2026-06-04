---
title: Callback Usage
summary: Demonstrates the use of callbacks
component: Callbacks
reviewed: 2026-06-04
---

> [!WARNING]
> Callbacks should only be used in exceptional situations, for example, to introduce messaging behind a synchronous API in a legacy component that can't be changed. They allow gradually transitioning applications towards messaging. See the [when to use callbacks](/nservicebus/messaging/callbacks.md#when-to-use-callbacks) section for more information.

Callbacks allow a sender to receive a response value directly from a message handler, turning a one-way send into a request/reply interaction. This sample demonstrates callbacks returning an enum, an integer, and an object.

## Running the sample

 1. Run the solution. Two console applications will start.
 1. In the Sender console application, press various keys to trigger each scenario when prompted.


## Shared project

A class library containing the messages and shared code.


### Status enum

This is used for the enum callback scenario.

```csharp
public enum Status
{
    OK,
    Error
}
```

## Sender project

A console application responsible for sending messages and handling the callback from the reply.


### Send and callback for an enum

snippet: SendEnumMessage


### Send and callback for an int

snippet: SendIntMessage


### Send and callback for an object

snippet: SendObjectMessage


## Receiver project

A console application responsible for replying to messages from the Sender.


### Return an enum

snippet: EnumMessageHandler


### Return an int

snippet: IntMessageHandler


### Return an object

Note that this scenario requires a `Reply` with a real message.

snippet: ObjectMessageHandler
