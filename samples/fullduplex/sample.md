---
title: Full Duplex
summary: Using full-duplex and request/response communication.
reviewed: 2026-01-05
component: Core
redirects:
- nservicebus/full-duplex-sample
---

> [!NOTE]
> [Sagas](/nservicebus/sagas/) is a better approach when responses must be correlated to requests as they provide message-to-state correlation out of the box.

Run the solution. Two console applications start-up, `Client` and `Server.`

## Code walk-through

### Messages

Look at the messages in the `Shared` project:

snippet: RequestMessage

snippet: ResponseMessage

The two classes here implement the NServiceBus `IMessage` interface, indicating they are messages. These classes only have properties, each with get and set access. The `RequestDataMessage` is sent from the client to the server, and the `DataResponseMessage` replies from the server to the client.

### Client

The client console has an input loop:

snippet: ClientLoop

This code sends a message with a new `Guid` and a string value every time the 'Enter' key is pressed.

### Server

When a `RequestDataMessage` arrives in the server queue, the bus dispatches it to the message handler found in the `RequestDataMessageHandler.cs` file in the `Server` project. The bus knows which classes to call based on the interface they implement.

snippet: RequestDataMessageHandler

At start-up, the bus scans all assemblies and builds a dictionary indicating which classes handle which messages. So when a given message arrives in a queue, the bus knows which class to invoke.

The `Handle` method of this class contains this:

snippet: DataResponseReply

Finally, the bus replies with the response message. The bus knows to send the responses to where the message was sent from, every time it receives a message from the queue.

Open `DataResponseMessageHandler.cs` in the `Client` project and find a class whose signature looks similar to the message handler on the server:

snippet: DataResponseMessageHandler
