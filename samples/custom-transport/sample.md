---
title: Building a transport
summary: Building a Transport using the file system as a store.
reviewed: 2016-03-21
component: Core
related:
- nservicebus/transports
---

This sample show how to build a Transport using the file system as a data store.


DANGER: This is for learning purposes only and NOT for use in production.


## Sample Structure

The sample has two endpoint, `Endpoint1` and `Endpoint2`.

Both endpoints are configured to use the `FileTransport`.

snippet: UseDefinition


### Message Flow


#### Endpoint1 Starts

`Endpoint1` starts the message flow with a send of `MessageA` to `Endpoint2`.

snippet: StartMessageInteraction


#### Endpoint2 Handles and Replies

`Endpoint2` has a Handler for `MessageA` and replies with `MessageB`

snippet: MessageAHandler


#### Endpoint1 Handles the reply

`Endpoint1` then handles that reply.

snippet: MessageBHandler


## Running the Sample

Start the sample with both `Endpoint1` and `Endpoint2` as startup projects. This will Force both endpoints to create their underlying file system queues.

Now start `Endpoint1`. Notice it will send a message at startup.

Now look at the file system for `Endpoint2` `%temp%\FileTransport\Samples.CustomTransport.Endpoint2`. It will contain the message headers in the root and the message body in `%temp%\FileTransport\Samples.CustomTransport.Endpoint2\.bodies`.

Now start `Endpoint2`. The reply message will appear in its file system `%temp%\FileTransport\Samples.CustomTransport.Endpoint2\`.


## Transport Implementation


### Transport definition

The `TransportDefinition` allows a Transport to define how it interacts with the core of NServiceBus.

snippet: TransportDefinition


### Storage location

This transport is hard coded to persist message to `%TEMP%FileTransport/ADDRESS/`.

snippet: BaseDirectoryBuilder


### Header Serializer

To serialize [Headers](/nservicebus/messaging/headers.md) headers this Transport uses JSON via[DataContractJsonSerializer](https://msdn.microsoft.com/en-us/library/system.runtime.serialization.json.datacontractjsonserializer.aspx)

snippet: HeaderSerializer


### QueueCreation

At startup a transport can optionally [create queues](/nservicebus/transports/queuecreation.md).

snippet: QueueCreation


### DirectoryBasedTransaction

How a Transport handles transactions differs greatly between specific implementations. For demonstration purposes this Transport uses a highly simplified file system based transaction.

snippet: DirectoryBasedTransaction


### Dispatcher

The dispatcher is responsible for translating a message (its binary body and headers) and placing it onto the underlying transport technology.

snippet: Dispatcher


### MessagePump

The message pump is responsible for reading message from the underlying transport and pushing them into the [Message Handling Pipeline](/nservicebus/pipeline/).

snippet: MessagePump