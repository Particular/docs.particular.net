---
title: Using NServiceBus in an ASP.NET Core WebAPI Application sample
summary: NServiceBus sample that illustrates how to send messages to an endpoint from a ASP.NET Core WebAPI application.
component: Core
reviewed: 2024-12-24
related:
- nservicebus/hosting
---

include: webhost-warning

This sample shows how to send messages to an NServiceBus endpoint from an ASP.NET Core WebAPI application.

## Running the solution

When the solution is run, a new browser window/tab opens, as well as a console application. The browser will navigate to `http://localhost:58118/`.

An async [WebAPI](https://dotnet.microsoft.com/apps/aspnet/apis) controller handles the request. It creates an NServiceBus message and sends it to the endpoint running in the console application. The message has been processed successfully when the console application prints "Message received at endpoint".

## Prerequisites

- Visual Studio 2019 is required to run this sample.

### Initialize the WebAPI endpoint

Open `Program.cs` and look at the `UseNServiceBus` method.

A [Send-Only endpoint](/nservicebus/hosting/#self-hosting-send-only-hosting) named `Samples.ASPNETCore.Sender` is configured in the WebAPI application by creating a new `EndpointConfiguration`.

snippet: EndpointConfiguration

Routing is configured to send every message from the assembly containing `MyMessage` to the `Samples.ASPNETCore.Endpoint` endpoint.

### Injection into the Controller

The message session is injected into `SendMessageController` via constructor injection.

snippet: MessageSessionInjection

### Sending the message

Send messages through the `IMessageSession` instance provided by ASP.NET Core.

snippet: MessageSessionUsage

### Processing the message

The message is picked up and processed by a message handler in the `Samples.ASPNETCore.Endpoint` endpoint.

snippet: MessageHandler
