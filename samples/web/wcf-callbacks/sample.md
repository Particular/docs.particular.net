---
title: WCF request response via callbacks
summary: Mapping between WCF and messages on the bus via the callbacks API.
reviewed: 2018-02-23
component: Callbacks
related:
- samples/web/owin-pass-through
---


## Introduction

This sample shows how to perform a WCF request response by leveraging an NServicebus [Callback](/nservicebus/messaging/callbacks.md).

include: callbacks-disclaimer

## WCF helpers


### Shared contract

A generic interface that is shared between both Client and Server to give a strong typed API.

snippet: ICallbackService

Note: For simplicity, this interface is located in the same assembly as the server-side helpers. This results in a reference to NServiceBus assemblies on the client side. In a real world solution this interface would most likely be moved to another assembly to avoid the need for a NServiceBus reference on the client side.


### Receiving endpoint helpers


#### WCF mapper

Maps a request-response message pair to a [ServiceHost](https://msdn.microsoft.com/en-us/library/system.servicemodel.servicehost.aspx) listening on a [BasicHttpBinding](https://msdn.microsoft.com/en-us/library/system.servicemodel.basichttpbinding.aspx).

The url used for the binding will be of the format `http://localhost:8080/BusService/{RequestMessage}_{Response}`. For a message `EnumMessage` that has a response of `Status` the url would be `http://localhost:8080/BusService/EnumMessage_Status`

If a different binding or url structure is required it can be customized:

snippet: WcfMapper


#### CallbackService

The server side implementation of `ICallbackService`. This class handles the correlation of the request to the response.

Note: In NServiceBus version 5 and below, the callback APIs for Enums, Ints and message responses differ slightly. Some logic is required to call the correct API for each response type. In version 6 and above this API has been simplified and no logic is required.

snippet: CallbackService


### Client helpers


#### ClientChannelBuilder

The `ClientChannelBuilder` creates a proxy at run-time to allow strong typed execution of a mapped WCF service.

snippet: ClientChannelBuilder

If generating a static proxy, using the Visual Studio "Add Service Reference" feature, no `ClientChannelBuilder` is required.


## Receiving endpoint configuration


### Mapping specific request-response pairs

This method maps some specific known request-response pairs to be listened to via a given url prefix.

snippet: startwcf


### Apply mapping to endpoint

Apply the request-response at bus startup.

snippet: startbus


## Client configuration


### SendHelper

A helper that builds and cleans up both the [ChannelFactory](https://msdn.microsoft.com/en-us/library/ms576132.aspx) and the channel.

snippet: SendHelper

Note: For the purposes of this sample, a new `ChannelFactory` and `ICommunicationObject` is created for every call. Depending on the specific use case it may be required to apply different scoping, lifetime and cleanup rules for these instances.


### Sending

The request, sending, and handling of the response.

snippet: Send
