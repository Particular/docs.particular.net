---
title: WCF request response via Callbacks
summary: Mapping between WCF and messages on the bus via the Callbacks API.
reviewed: 2016-03-21
component: Callbacks
related:
- samples/web/owin-pass-through
---


## Introduction

This samples shows how to perform a WCF request response by leveraging [Callback](/nservicebus/messaging/callbacks.md) of NServiceBus.


## WCF Helpers


### Shared contract

An generic interface that is shared between both Client and Server to give a strong typed API.

snippet: ICallbackService

Note: For the sake of simplicity this interface is located in the same assembly as the server side helpers. This results in a reference to NServiceBus assemblies on the client side. In a real world solution this interface would most likely be moved to another assembly to avoid the need for a NServiceBus reference on the client side.


### Receiving Endpoint Helpers


#### WCF Mapper

Maps a Request-Response message pair to a  [ServiceHost](https://msdn.microsoft.com/en-us/library/system.servicemodel.servicehost.aspx) listening on a [BasicHttpBinding](https://msdn.microsoft.com/en-us/library/system.servicemodel.basichttpbinding.aspx).

The url used for the binding will be of the format `http://localhost:8080/BusService/{RequestMessage}_{Response}`. So for a message `EnumMessage` that has a response of `Status` the url would be `http://localhost:8080/BusService/EnumMessage_Status`

If a different binding or url structure is required it can be customized:

snippet: WcfMapper


#### CallbackService

The server side implementation of `ICallbackService`. This class handles the correlation of Request to Response.

Note: In Version 5 and below the Callback APIs for Enums, Ints and message responses differ slightly hence some logic is required to call the correct API for each response type. In Version 6 and above this API has been simplified and hence no logic is required.

snippet: CallbackService


### Client Helpers


#### ClientChannelBuilder

The `ClientChannelBuilder` creates a proxy at run time to allow strong typed execution of a mapped WCF service.

snippet: ClientChannelBuilder

If generating a static proxy, using the Visual Studio "Add Service Reference" feature, no `ClientChannelBuilder` is required.


## Receiving Endpoint Configuration


### Mapping Specific Request-Response pairs

This method maps some specific known Request-Response pairs to be listened to via a given url prefix.

snippet: startwcf


### Apply mapping to endpoint

Apply the Request-Response at bus startup.

snippet: startbus


## Client Configuration


### SendHelper

A helper that build and cleans up both the [ChannelFactory](https://msdn.microsoft.com/en-us/library/ms576132.aspx) and the channel.

snippet: SendHelper

Note: For the purposes of this sample, for every call it creates a new `ChannelFactory` and `ICommunicationObject`. Depending on the specific use case it may be required to apply different scoping, lifetime and cleanup rules for these instances.


### Sending

The request send and handling of the response.

snippet: Send