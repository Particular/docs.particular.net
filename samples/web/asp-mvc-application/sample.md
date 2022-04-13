---
title: Using NServiceBus with ASP.NET MVC
summary: Integrating NServiceBus with ASP.NET MVC web applications to send messages from a website.
reviewed: 2022-04-13
component: Core
redirects:
- nservicebus/using-nservicebus-with-asp.net-mvc
related:
- nservicebus/hosting
- nservicebus/hosting/publishing-from-web-applications
---

This sample consists of a web application hosting Web API controllers and a console application hosting the NServiceBus endpoint. The web application sends a command to the endpoint, waits for a response, and returns the result to the user. Web application shows two methods of sending a command:

 * `SendAndBlock`: a method in synchronous `Controller` class  
 * `SendAsync`: a method in asynchronous `AsyncController` class

NOTE: In `SendAndBlock`, the web page renders synchronously. From the user's perspective, the interaction is synchronous and blocking, even though behind the scenes NServiceBus is messaging asynchronously.

After running, the web application renders the following page:

![AsyncPages Asp.Net Mvc sample running](async-pages-mvc-selecting-blocking-method.png "AsyncPages Asp.Net Mvc sample running")

Choosing SendAsync results in the following page:

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


## Sending a message


### Asynchronous controller

Using `AsyncController`:

snippet: AsyncController


### Synchronous controller

Open the `SendAndBlockController` class:

snippet: SendAndBlockController

The controller is sending a command using the instance of `IMessageSession` injected via the constructor. The code calls the `Request` method, passing in the newly created command instance.

A calling to the `Request` method returns only after a response from the handler is received.


## Handling the message

In the Server project, open the `CommandMessageHandler` class to see the following:

snippet: CommandMessageHandler

This class implements the NServiceBus interface `IHandleMessages<T>` where `T` is the specific message type being handled; in this case, the `Command` message.

NServiceBus manages the classes that implement this interface. When a message arrives in the input queue, it is deserialized, and then, based on its type, NServiceBus instantiates the relevant message handler classes and calls their `Handle` method, passing in the message object and the context object.

In the method body notice the response being returned to the originating endpoint. This will result in a message being added to the input queue for `AsyncPagesMVC` endpoint.
