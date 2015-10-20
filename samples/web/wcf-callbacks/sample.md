---
title: WCF calls via Callbacks
summary: Illustrates how to map between WCF and messages on the bus via the Callbacks API.
tags:
related:
- nservicebus/messaging/handling-responses-on-the-client-side
---


## Introduction

This samples shows how to perform a WCF request response by leveraging the [callback API](/nservicebus/messaging/handling-responses-on-the-client-side.md) of NServiceBus.


## WCF Helpers


### Shared contract

An generic interface that is share between both Client and Server to give a strong typed API. 

<!-- import ICallbackService -->


### Receiving Endpoint Helpers


#### WCF Mapper

Maps a Request-Response message pair to a  [ServiceHost](https://msdn.microsoft.com/en-us/library/system.servicemodel.servicehost.aspx) listening on [BasicHttpBinding](https://msdn.microsoft.com/en-us/library/system.servicemodel.basichttpbinding.aspx).

The url used for the binding will be of the format `http://localhost:8080/BusService/{RequestMessage}_{Response}`. So for a message `EnumMessage` that has a response of `Status` the url would be `http://localhost:8080/BusService/EnumMessage_Status`

If you want to use a different binding or url structure you can customize this code.

<!-- import WcfMapper -->


#### CallbackService

The server side implementation of `ICallbackService`. This class handles the actual correlation of Request to Response. Note that the Callback APIs for Enums, Int sand message responses differ slightly.

<!-- import CallbackService -->


### Client Helpers


#### ClientChannelBuilder

The ClientChannelBuilder creates a proxy at run time to allow strong typed execution of a mapped WCF service.

<!-- import ClientChannelBuilder -->

If you are generating a static proxy, using the Visual Studio "Add Service Reference" feature, you wont need to use `ClientChannelBuilder`.


## Receiving Endpoint Configuration


### Mapping Specific Request-Response pairs

This method maps some specific known Request-Response pairs to be listened to via a given url prefix.

<!-- import startwcf -->


### Apply mapping to endpoint

Apply the Request-Response at bus startup.

<!-- import startbus -->


## Client Configuration


### SendHelper

A helper that build and cleans up both the [ChannelFactory](https://msdn.microsoft.com/en-us/library/ms576132.aspx) and the channel.

<!-- import SendHelper -->

Note that, for the purposes of this sample, for every call it creates a new `ChannelFactory` and `ICommunicationObject`. Depending on your specific use case you mat want to apply different scoping, lifetime and cleanup rules for these instances.


### Sending

The actual request send and handling of the response.

<!-- import Send -->

