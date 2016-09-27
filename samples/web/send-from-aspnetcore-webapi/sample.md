---
title: Using NServiceBus in an ASP.NET Core WebAPI Application
component: Core
reviewed: 2016-09-27
related:
- nservicebus/hosting
---

Run the solution, a new browser window/tab opens, as well as a console application.

The browser will open up the URL "http://localhost:51863/api/sendmessage". An async WebAPI controller handles the request. It creates an NServiceBus message and sends it to the endpoint running in the console application. The message has been processed successfully when the console application prints "Message received at endpoint".  


### Initialize the WebAPI endpoint

Open `Startup.cs` and look at the `ConfigureServices` method.

A "Send-Only" endpoint named `Samples.ASPNETCore.Sender` is configured in the WebAPI application by creating a new `EndpointConfiguration`. 

snippet:EndpointConfiguration

Routing is configured to send every message from the assembly containing `MyMessage` to the `Samples.ASPNETCore.Endpoint` endpoint.

snippet:Routing

The endpoint is started. At this point, the configuration is locked.

snippet:EndpointStart

Finally, the endpoint is registered as a singleton instance of type `IMessageSession` in ASP.NET Cores `ServiceCollection`., ready to be injected into the controller.

snippet:ServiceRegistration

### Injection into the Controller

The endpoint instance is injected into the `SendMessageController` at construction time by ASP.NET Core.

snippet:MessageSessionInjection

### Sending the message 

Send and await messages through the `IMessageSession` instance provided by ASP.NET Core.

snippet:MessageSessionUsage


### Processing the message 

The message is picked up and processed by a message handler in the `Samples.ASPNETCore.Endpoint` endpoint. 

snippet:MessageHandler