---
title: Near Real Time Transient Clients
summary: Shows how to relay NServiceBus events to occasionally-connected clients via SignalR.
reviewed: 2017-05-22
component: Core
related: 
 - nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed
 - nservicebus/messaging/unobtrusive-mode
 - samples/showcase
---

For near real-time, occasionally connected clients, messages are only relevant for a short period of time. Clients that received near real-time stock ticker updates are a common example of these types of clients.

One of the basic features of message queuing is the ability for the receiving endpoints to maintain service even when offline.  In this scenario, the long lasting, durable nature of message queuing can result in a backlog of irrelevant messages, which if disconnected long enough can result in queue quotas being exceeded, and can ultimately result in exceptions on the message sender possibly impacting other parts of the system.

A possible answer is to [unsubscribe](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md#manually-subscribing-to-a-message) on shutdown, which is fragile since the client may not successfully complete the unsubscribe when a crash occurs.

Another solution is to avoid implementing each client as an NServiceBus endpoint, but instead use a push technology, such as [SignalR](http://signalr.net/), to update clients only while they are connected.

This sample demonstrates how to use a SignalR server, which also acts as an NServiceBus endpoint, to push subscribed NServiceBus events to any connected SignalR clients.

## Sample Project Structure

Before running the sample, look over the solution structure, the projects, and the classes. The projects `Publisher` and `ClientHub` are each command-line apps that host an instance of NServiceBus.

The `ClientHub` project also hosts a SignalR server. The `Client` project is a command line app that hosts a SignalR client.



## Sharing Message Types with SignalR

The `StockEvents` project contains the definition a message class that is shared with both NServiceBus endpoints, the SignalR hub, and the SignalR client. Open "StockTick" to see the message that will be published by this sample. Note that this event is defined using [Unobtrusive Mode Message Conventions](/nservicebus/messaging/unobtrusive-mode.md), allowing the `Client` project, which only uses SignalR, to reference the message type without needing a reference to NServiceBus.

snippet: MessageConventionsForNonNSB



## Hosting SignalR with NServiceBus

This sample shows how to host a self-hosted SignalR 2 server side-by-side with a NServiceBus endpoint. For more information on using SignalR Self-Host see the Microsoft tutorial [SignalR Self-Host](https://docs.microsoft.com/en-us/aspnet/signalr/overview/deployment/tutorial-signalr-self-host).



## Bridging the Bus to Clients using SignalR

The `ClientHub` project subscribes to the `StockTick ` event published by `Publisher`. 

`StockTickHub` defines an `async` method, `ForwardStockTick` that sends the `StockTick` message to the connected SignalR clients.

snippet: StockTickHub

When the `StockTick` event is handled, it invokes the `ForwardStockTick` method on the `StockTickHub`.

snippet: StockTickHandler



## Web Applications

In this sample, the SignalR client is implemented as a .NET console application. The client could just as easily be implemented using the JavaScript SignalR client hosted in a web application.



## Scaling Out SignalR with NServiceBus

When the number of connected clients exceeds the capability of a single SignalR server, it will be necessary to scale out the SignalR server. Scaling out SignalR is described in the [Introduction to Scaleout in SignalR](https://docs.microsoft.com/en-us/aspnet/signalr/overview/performance/scaleout-in-signalr) article by Microsoft.

Since each SignalR server can also be an NServiceBus subscriber, each scaled out instance of the server also receives and pushes events to its connected clients. This, along with a load balancer to distribute clients between the servers, is sufficient as long as the SignalR server is simply relaying NServiceBus events to the clients.

If SignalR usage extends beyond simply pushing NServiceBus events, then scaling out using a backplane will likely be necessary. In this case, using a backplane will result in duplicate messages, since each SignalR server is also a subscriber and will forward pushed events to other SignalR servers on the backplane. Instead, only deploy a single SignalR server that is also an NServiceBus subscriber. This will prevent duplicates, but at the expense of an additional message hop as the message is forwarded through the backplane to the clients.
