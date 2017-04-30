---
title: Using NServiceBus in an ASP.NET Core WebAPI Application
summary: Illustrates how to send messages to a NServiceBus endpoint from a ASP.NET Core WebAPI application.
component: Core
reviewed: 2017-04-27
related:
- nservicebus/hosting
---


This sample shows how to send messages to an NServiceBus endpoint from an ASP.NET Core WebAPI application. 

WARNING: This sample runs on the full .NET Framework 4.6.1 but utilizes both .NET Standard and .NET Framework dependencies, i.e. ASP.NET Core and NServiceBus. So while .NET Standard dependencies can be consumed by the .NET Framework runtime, the .NET Core runtime is not currently supported.


## Using the full .NET Framework runtime

The default *ASP.NET Core Web Application (.NET Framework)* Visual Studio 2017 project template is used.


## Running the solution

When the solution is run, a new browser window/tab opens, as well as a console application. The browser will navigate to `http://localhost:51863/api/sendmessage`.

An async [WebAPI](https://www.asp.net/web-api) controller handles the request. It creates an NServiceBus message and sends it to the endpoint running in the console application. The message has been processed successfully when the console application prints "Message received at endpoint". 


## Prerequisites

  - Visual Studio 2017 is required to run this sample.

### Initialize the WebAPI endpoint

Open `Startup.cs` and look at the `ConfigureServices` method.

A [Send-Only endpoint](/nservicebus/hosting/#self-hosting-send-only-hosting) named `Samples.ASPNETCore.Sender` is configured in the WebAPI application by creating a new `EndpointConfiguration`.

snippet: EndpointConfiguration

Routing is configured to send every message from the assembly containing `MyMessage` to the `Samples.ASPNETCore.Endpoint` endpoint.

snippet: Routing

The endpoint is started. At this point, the configuration is locked.

snippet: EndpointStart

Finally, the endpoint is registered as a singleton instance of type `IMessageSession` in ASP.NET Cores `ServiceCollection`, ready to be injected into the controller.

An alternative would be to register the instance as type `IEndpointInstance`. `IMessageSession` is a leaner interface, containing only the methods necessary to send/publish messages. It is a good choice for [sending messages outside message handlers](/nservicebus/upgrades/5to6/moving-away-from-ibus.md#migrating-away-from-ibus-sending-messages-outside-message-handlers) if no endpoint management functionality is required.

snippet: ServiceRegistration


### Injection into the Controller

The endpoint instance is injected into the `SendMessageController` at construction time by ASP.NET Core.

snippet: MessageSessionInjection


### Sending the message 

Send and await messages through the `IMessageSession` instance provided by ASP.NET Core.

snippet: MessageSessionUsage


### Processing the message 

The message is picked up and processed by a message handler in the `Samples.ASPNETCore.Endpoint` endpoint.

snippet: MessageHandler
