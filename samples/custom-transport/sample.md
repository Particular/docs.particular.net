---
title: Building a transport
summary: Building a Transport using the file system as a store.
reviewed: 2017-11-14
component: Core
related:
- transports
---

This sample show how to build a transport using the file system as a data store.


DANGER: This is for learning purposes only and NOT for use in production.

**This sample requires Visual Studio 2017.**


## Sample Structure

The sample has two endpoints, `Endpoint1` and `Endpoint2`.

Both endpoints are configured to use the `FileTransport`.

snippet: UseDefinition


### Message Flow


#### Endpoint1 Starts

`Endpoint1` starts the message flow with a send of `MessageA` to `Endpoint2`.

snippet: StartMessageInteraction


#### Endpoint2 Handles and Replies

`Endpoint2` has a Handler for `MessageA` and replies with `MessageB`

snippet: MessageAHandler


#### Endpoint1 handles the reply

`Endpoint1` then handles that reply.

snippet: MessageBHandler


## Running the sample

Start the sample with both `Endpoint1` and `Endpoint2` as startup projects. This will force both endpoints to create their underlying file system queues.

Now start `Endpoint1`. Notice it will send a message at startup.

Now look at the file system for `Endpoint2` `%temp%\FileTransport\Samples.CustomTransport.Endpoint2`. It will contain the message headers in the root and the message body in `%temp%\FileTransport\Samples.CustomTransport.Endpoint2\.bodies`.

Now start `Endpoint2`. The reply message will appear in its file system `%temp%\FileTransport\Samples.CustomTransport.Endpoint2\`.


## Transport implementation


### Transport definition

The `TransportDefinition` allows a transport to define how it interacts with the core of NServiceBus.

snippet: TransportDefinition


### Storage location

This transport is hard coded to persist messages to `%TEMP%FileTransport/ADDRESS/`.

snippet: BaseDirectoryBuilder


### Header serializer

To serialize [headers](/nservicebus/messaging/headers.md) this transport uses JSON via [DataContractJsonSerializer](https://msdn.microsoft.com/en-us/library/system.runtime.serialization.json.datacontractjsonserializer.aspx)

snippet: HeaderSerializer


### Queue creation

At startup a transport can optionally [create queues](/transports/queuecreation.md).

snippet: QueueCreation


### Handling transactions

How a transport handles transactions differs greatly between specific implementations. For demonstration purposes this transport uses a highly simplified file system based transaction.

snippet: DirectoryBasedTransaction


### Dispatcher

The dispatcher is responsible for translating a message (its binary body and headers) and placing it onto the underlying transport technology.

snippet: Dispatcher


### Message pump

The message pump is responsible for reading message from the underlying transport and pushing them into the [message handling pipeline](/nservicebus/pipeline/).

snippet: MessagePump


## Transport tests

NServiceBus provides a test suite targeting transport implementations to verify the implementation.


### Pulling in the tests

The tests are shipped in the `NServiceBus.TransportTests.Sources` NuGet package. This package can be installed into a dedicated test project. In this sample, `CustomTransport.TransportTests` contains the transport tests.


### Configuring the tests

The transport tests need to be configured to use the custom transport by implementing `IConfigureTransportInfrastructure`:

snippet: TransportTestConfiguration


### Running the tests

The transport tests can be run with all test runners that support NUnit.
