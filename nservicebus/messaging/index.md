---
title: Messaging
summary: Outline the various approach to sending-receiving, defining messages and common messaging patterns.
reviewed: 2016-11-29
---

Messages are discrete units of data communicated to one or more recipients using a predefined contract. Messaging are a common way for systems or applications to interact, integration. Given the interactions are done over a network. Asynchronous is a messaging is a well known pattern to communicate or exchange data over the network. Link to EIP

NServiceBus provides a means and standards for implementing indirect communication of messages between processes in .NET.

## Durable

Explain durable messaging. Send them to Bus vs Broker with a quick overview (bus is distributed fault tolerant / broker is centralized single point of failure)

Small diagram for durable messaging (like email)

## Physical, Logical, and Native Messages

Messsaging protocols often make a distinction between a physical message and the logical message.

The native message is the protocol/transport specific message format required by the underlying messaging technology.
- Drop

The physical message contains the logical message as well message metadata.
- Headers
- Routing
- Serialized Logical Message

The logical message is the portion of the physical message that contains the bytes that will actually be consumed by the user-code portion of the process, these bytes in NServiceBus being a serialized version of an object. 
- Serialized / Deserialized

![Physical and Logical relationships](https://www.lucidchart.com/publicSegments/view/bf045538-99f3-4e49-9f79-7315d80fde61/image.png)

## Defining Messages

Messages in NServiceBus are defined by implementing a .NET class or interface with properties.  The namespace and type provide a way to distinguish one message from another.

## Routing and Message Patterns

Messages must know how to 
It relies on an intermediate storage/transfer technology to handle the communication between the sender and the receiver, so neither is blocked by the other. These messages are routed between common channels that d

The routing of messages between recipients has a number of patterns. NServiceBus supports 3 of these:

### Commands

Commands are one way messages which are sent to a single recipient and no direct response is expected.

### Publish/Subscribe

Publish/Subscribe, or Pub/Sub, is a pattern where zero or more recipients subscribe to recieve a copy of a message that is published (also called an Event), but no response is  (in NServiceBus described by the logical message C# Type) and each will recieve a copy of the message. No response is expected.

### Reply

Full duplex message, as variation of Commands, are likewise sent to a single recipient, but a direct response is expected.
