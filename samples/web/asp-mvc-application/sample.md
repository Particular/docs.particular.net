---
title: Using NServiceBus with ASP.NET MVC
summary: Integrating NServiceBus in ASP.NET MVC web applications, to be able to send messages from the website.
reviewed: 2016-03-21
component: Core
redirects:
- nservicebus/using-nservicebus-with-asp.net-mvc
related:
- nservicebus/hosting
- nservicebus/hosting/publishing-from-web-applications
---

Run the solution. A new browser window/tab opens, as well as a console application.

The web sample starts with two available methods of sending a command to the server and waiting for a response from it:

 * `SendAndBlock`: a controller uses NServiceBus
 * `SendAsync`: an `AsyncController` uses NServiceBus

The sample covers only the sending of the asynchronous message as the send and block are similar in NServiceBus.

NOTE: In `SendAndBlock`, the web page renders synchronously. From the user's perspective, the interaction is synchronous and blocking, even though behind the scenes NServiceBus is messaging asynchronously.

![AsyncPages Asp.Net Mvc sample running](async-pages-mvc-selecting-blocking-method.png "AsyncPages Asp.Net Mvc sample running")

Choosing SendAsync results in the following page:

![AsyncPages Asp.Net Mvc sample running](async-pages-mvc-running.png "AsyncPages Asp.Net Mvc sample running")

Changing the number in the text box from even to odd changes the result.

Now, look at the code. This sample has three projects:

 * `AsyncPagesMvc`: ASP.NET MVC application that sends messages (found in `Messages` project)
 * `Shared`: Common code including declaration of messages
 * `Server`: Destination of messages sent from the MVC  project. Hosted in a console application


## Initializing the bus

The sample controllers hold a reference of the bus, which is used later to send messages and receive a response.

In `AsyncPagesMvc`, open `Global.asax.cs` and see the code for the `ApplicationStart` method:

snippet: ApplicationStart

For more details on how to inject NServiceBus classes into the controllers, check the [Sending from an ASP.NET MVC Controller](/samples/web/send-from-mvc-controller/).


## Sending a message


### Asynchronous message sending: SendAsync controller

Using `AsyncController`:

snippet: AsyncController


### Synchronous message sending: SendAndBlockController controller (version 4-5)

Open the SendAndBlockController class:

snippet: SendAndBlockController

The controller is referencing its `IBus` (NServiceBus injected it when the controller was instantiated). The code calls the send method, passing in the newly created command object. The bus isn't anything special in the code; it's just an object for calling methods.

The call registers a callback method that will be called (with this parameter) as soon as a response is received by the server.


## Handling the message

In the Server project, open the `CommandMessageHandler` class to see the following:

snippet: CommandMessageHandler

This class implements the NServiceBus interface `IHandleMessages<T>` where `T` is the specific message type being handled; in this case, the Command message.

NServiceBus manages the classes that implement this interface. When a message arrives in the input queue, it is deserialized, and then, based on its type, NServiceBus instantiates the relevant classes and calls their Handle method, passing in the message object.

In the method body notice the response being returned to the originating endpoint. This will result in a message being added to the input queue for `AsyncPagesMVC`.