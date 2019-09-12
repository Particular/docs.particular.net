---
title: Using NServiceBus in an ASP.NET Core WebAPI Application
summary: Illustrates how to send messages to a NServiceBus endpoint from a ASP.NET Core WebAPI application.
component: Core
reviewed: 2019-02-06
related:
- nservicebus/hosting
---


This sample shows how to send messages to an NServiceBus endpoint from an ASP.NET Core WebAPI application.

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

Finally, NServiceBus is configured using the `AddNServiceBus` method. This sets up NServiceBus as a [hosted service](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services) and registers the `IMessageSession` in the dependency injection container so that controllers can take a dependency on it.

snippet: ServiceRegistration

### Injection into the Controller

The message session is injected into `SendMessageController` via constructor injection.

snippet: MessageSessionInjection

### Sending the message

Send messages through the `IMessageSession` instance provided by ASP.NET Core.

snippet: MessageSessionUsage

### Processing the message

The message is picked up and processed by a message handler in the `Samples.ASPNETCore.Endpoint` endpoint.

snippet: MessageHandler
