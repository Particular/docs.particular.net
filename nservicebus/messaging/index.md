---
title: Messaging
summary: Outline the various approach to sending-receiving, defining messages and common messaging patterns.
reviewed: 2016-11-29
---

Messages are discrete units of data communicated to one or more recipients using a predefined contract. Messages are a common way for systems or applications to interact, indeed the most common communications protocols on the Internet use messaging at their heart. HTTP protocol describes both a request message and the response message. Even SMTP is a messaging based system.

Communication via messaging occurs either directly, as in the case of HTTP, or indirectly in the case of SMTP. The difference being whether the producer and/or consumer of the message is blocked during transfer and/or consumption of the message.

NServiceBus provides a means and standards for implementing indirect communication of messages between processes in .NET.

## Physical, Logical, and Native Messages

Messsaging protocols often make a distinction between a physical message and the logical message.

The native message is the protocol/transport specific message format required by the underlying messaging technology.

The physical message contains the logical message as well message metadata.

The logical message is the portion of the physical message that contains the bytes that will actually be consumed by the user-code portion of the process, these bytes in NServiceBus being a serialized version of an object.

![Physical and Logical relationships](https://www.lucidchart.com/publicSegments/view/bf045538-99f3-4e49-9f79-7315d80fde61/image.png)

## Defining Messages

Messages in NServiceBus are defined by implementing a .NET class or interface with properties.  The namespace and type provide a way to distinguish one message from another.

## Routing and Message Patterns

Messages must know how to 
It relies on an intermediate storage/transfer technology to handle the communication between the sender and the receiver, so neither is blocked by the other. These messages are routed between common channels that d

The routing of messages between recipients has a number of patterns. NServiceBus supports 3 of these:

### Commands

Commands are one way messages which are sent to a single recipient and no direct response is expected.

### Full Duplex

Full duplex message, as variation of Commands, are likewise sent to a single recipient, but a direct response is expected.

### Publish/Subscribe

Publish/Subscribe, or Pub/Sub, is a pattern where zero or more recipients subscribe to recieve a copy of a message that is published (also called an Event), but no response is  (in NServiceBus described by the logical message C# Type) and each will recieve a copy of the message. No response is expected.


