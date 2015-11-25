---
title: Using NServiceBus with ASP.NET MVC
summary: Integrating NServiceBus in ASP.NET MVC web applications, to be able to send messages from the website.
tags: []
redirects:
- nservicebus/using-nservicebus-with-asp.net-mvc
related:
- nservicebus/hosting
- nservicebus/hosting/publishing-from-web-applications
---

Run the solution. A new browser window/tab opens, as well as a console application.

The web sample starts with two available methods of sending a command to the server and waiting for a response from it:

-   `SendAndBlock`: a controller uses NServiceBus
-   `SendAsync`: an `AsyncController` uses NServiceBus

The sample covers only the sending of the asynchronous message as the send and block are similar in NServiceBus.

NOTE: In `SendAndBlock`, the web page renders synchronously. From the user's perspective, the interaction is synchronous and blocking, even though behind the scenes NServiceBus is messaging asynchronously.

![AsyncPages Asp.Net Mvc sample running](async-pages-mvc-selecting-blocking-method.png "AsyncPages Asp.Net Mvc sample running")

Choosing SendAsync results in the following page: 

![AsyncPages Asp.Net Mvc sample running](async-pages-mvc-running.png "AsyncPages Asp.Net Mvc sample running")

Changing the number in the text box from even to odd changes the result.

Now, look at the code. This sample has three projects:
 
-   `AsyncPagesMvc`: ASP.NET MVC application that sends messages (found in `Messages` project)
-   `Shared`: Common code including declaration of messages
-   `Server`: Destination of messages sent from the MVC  project. Hosted in a console application

## Initializing the bus

The sample controllers hold a reference of the bus, which is used later to send messages and receive a response.

In `AsyncPagesMvc`, open `Global.asax.cs` and see the code for the `ApplicationStart` method:

snippet:ApplicationStart

By calling `With()`, the code indicates to NServiceBus to scan the directory where the web application is deployed (different from non-web applications).

The `.ForMvc` extension method injects `IBus` into the controllers by implementing the MVC interfaces `IDependencyResolver` and `IControllerActivator`.
 
The NServiceBus builder registers and instantiates `IControllerActivator` so that when the controllers are requested, the NServiceBus builder has the opportunity to inject the `IBus` implementation into their `IBus` public property.

Read [how the IBus is injected into the controllers](/samples/web/asp-mvc-injecting-bus/).

## Sending a message

### Asynchronous message sending: SendAsync controller

Using `AsyncController`:

snippet:AsyncController

### Synchronous message sending: SendAndBlockController controller

Open the SendAndBlockController class:

snippet:SendAndBlockController

The controller is referencing its `IBus` (NServiceBus injected it when the controller was instantiated). The code calls the send method, passing in the newly created command object. The bus isn't anything special in the code; it's just an object for calling methods.

The call registers a callback method that will be called (with this parameter) as soon as a response is received by the server.

## Handling the message

In the Server project, open the `CommandMessageHandler` class to see the following:

snippet:CommandMessageHandler

This class implements the NServiceBus interface `IHandleMessages<T>` where `T` is the specific message type being handled; in this case, the Command message.

NServiceBus manages the classes that implement this interface. When a message arrives in the input queue, it is deserialized, and then, based on its type, NServiceBus instantiates the relevant classes and calls their Handle method, passing in the message object.

Notice the `IBus` property of the class. This is how it gets a reference to the bus. In the method body you can see it calling the `Return` method on the bus, which results in a message being returned to `WebApplication`, specifically putting a message in the input queue whose name is determined by the namespace where the bus was configured; in this case, the `global.asax`: `AsyncPagesMVC`.
