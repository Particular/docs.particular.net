---
title: Transient Client Sample
reviewed: 2017-05-16
related: 
 - nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed
 - samples/show-case
---

This sample demonstrates occasionally connected clients that only need to received event updates while connected. In this case it is best not to connect the clients directly to your bus, but instead use a push technology, such as [SignalR](http://signalr.net/), to update clients while they are connected.

Clients that received stock ticker updates are a common usage of this pattern, and this sample provides a simple version of that scenario.

Before running the sample, look over the solution structure, the projects, and the classes. The projects `Publisher` and `ClientHub` each command line apps that host an instance of NServiceBus.

The `ClientHub` project also hosts a SignalR server. The `Client` project is a command line app that hosts a SignalR client.


## Sharing Message Types with SignalR

The `StockEvents` project contains the definition a message class that is shared with both NServiceBus endpoints, the SignalR hub, and the SignalR client. Open "StockTick" to see the message that will be published by this sample. Note that this event is defined using [Unobtrusive Mode Message Conventions](/nservicebus/messaging/unobtrusive-mode.md), allowing the `Client` project to reference the message type without needing a reference to NServiceBus.


## Hosting SignalR with NServiceBus

This sample shows how to host a self-hosted SignalR 2 server side-by-side with a NServiceBus endpoint. For more information on using SignalR Self-Host see the Microsoft tutorial ["SignalR Self-Host"](https://docs.microsoft.com/en-us/aspnet/signalr/overview/deployment/tutorial-signalr-self-host).


## Bridging the Bus to Clients using SignalR

The `ClientHub` project subscribes to the `StockTick ` event published by `Publisher`. 

`StockTickHub` defines an `async` method, `ForwardStockTick` that sends the `StockTick` message to the connected SignalR clients.

snippet: StockTickHub

When the `StockTick` event is handled, it invokes the `ForwardStockTick` method on the `StockTickHub`.

snippet: StockTickHandler


## Web Applications

In this sample the SignalR client is implemented as a .NET console application. The client could just as easily be implemented using the JavaScript client and used in a web application.


## Scaling Out SignalR with NServiceBus

When the number of connected clients exceeds the capability of a single SignalR server you will need to scale out SignalR. Using a backplane to scale out SignalR is described in the ["Introduction to Scaleout in SignalR"](https://docs.microsoft.com/en-us/aspnet/signalr/overview/performance/scaleout-in-signalr) article by Microsoft.

Since the SignalR server can also be NServiceBus subscribers, each scaled out instance of the server also receives and can forward the event. 