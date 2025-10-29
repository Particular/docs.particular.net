---
title: Using NServiceBus with ASP.NET MVC
summary: Integrating NServiceBus with ASP.NET MVC web applications to send messages from a website.
reviewed: 2025-10-29
component: Core
redirects:
- nservicebus/using-nservicebus-with-asp.net-mvc
related:
- nservicebus/hosting
- nservicebus/hosting/publishing-from-web-applications
redirects:
- samples/netcore-reference
---

This sample consists of an [ASP.NET MVC web application](https://dotnet.microsoft.com/en-us/apps/aspnet/mvc) hosting an NServiceBus endpoint and a console application hosting another NServiceBus endpoint. The web application sends a [command](/nservicebus/messaging/messages-events-commands.md#commands) to the console application, waits for a [reply](/nservicebus/messaging/reply-to-a-message.md), and returns the result to the user. 

After running, the web application renders the following page:

![AsyncPages Asp.Net Mvc sample running](async-pages-mvc-selecting-blocking-method.png "AsyncPages Asp.Net Mvc sample running")

This shows two methods for sending commands, which are [explained below](#sending-a-message):

 * `SendAndBlock`: a method in synchronous `Controller` class
 * `SendAsync`: a method in asynchronous `AsyncController` class

Clicking one of the links will render a page that allows you to send commands:

![AsyncPages Asp.Net Mvc sample running](async-pages-mvc-running.png "AsyncPages Asp.Net Mvc sample running")

Changing the number in the text box from even to odd changes the result.

## Structure

The solution in the sample consists of three projects:

 * `AsyncPagesMvc`: ASP.NET Core MVC application that sends messages
 * `Shared`: Common code including  messages types definitions
 * `Server`: Destination of messages sent from the MVC project. Hosted in a console application


## Initializing NServiceBus

In `AsyncPagesMvc`, open `Program.cs` and see the code for the `UseNServiceBus` method:

snippet: ApplicationStart

The call to `builder.UseNServiceBus(endpointConfiguration);` registers `IMessageSession` in the service collection, making it available for dependency injection into the controllers.

## Sending a message

The controller actions send a message to the console application using the `Request` method from the injected `IMessageSession` and wait to receive the reply.

The sample demonstrates the differences between sending a message with NServiceBus in an asynchronous versus synchronous controller action.

### Asynchronous controller

Using `AsyncController`:

snippet: AsyncController


### Synchronous controller

Open the `SendAndBlockController` class:

snippet: SendAndBlockController


The `Request` method returns once a response from the handler is received.

> [!NOTE]
> In `SendAndBlock`, the web page renders synchronously. From the user's perspective, the interaction is synchronous and blocking, even though behind the scenes NServiceBus is messaging asynchronously.

## Handling the message

In the Server project, open the `CommandMessageHandler` class to see the following:

snippet: CommandMessageHandler

`CommandMessageHandler` implements the NServiceBus interface `IHandleMessages<T>` where `T` is the message type being handled, in this case, the `Command` message.

 When a message arrives in the input queue it is deserialized and then, based on its type, NServiceBus instantiates the relevant `IHandleMessages<T>` implementations and calls their `Handle` methods passing in the message object and the context object.

Notice in the method body the response is being returned to the originating endpoint. This will result in the message being added to the input queue of the `AsyncPagesMVC` endpoint.
