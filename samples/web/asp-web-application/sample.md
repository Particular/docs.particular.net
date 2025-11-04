---
title: Using NServiceBus in an ASP.NET Core Web Application
component: Core
reviewed: 2025-10-29
redirects:
- nservicebus/using-nservicebus-in-a-asp.net-web-application
- samples/web/asp-mvc-application
- samples/web/blazor-server-application
- samples/web/send-from-aspnetcore-webapi
related:
- nservicebus/hosting
- nservicebus/hosting/publishing-from-web-applications
---

This sample shows how to send messages from an ASP.NET Core web application to an NServiceBus endpoint through several technologies:
- MVC
- Web Api
- Razor Pages
- Blazor

## Run the sample

There are three projects in the solution:
- `Server` - A console application which hosts the NServiceBus endpoint that handles messages sent from the `WebApp` project
- `WebApp` - An ASP.NET web application that sends messages to the `Server` endpoint using the technologies listed above
- `Shared` - A library which contains the message definition, shared by both the `Server` and `WebApp` projects

Both the `Server` and `WebApp` projects must be running.  When the `WebApp` is run, a browser window will open to display links for sending messages using different technologies.

Excluding the Web Api link, which sends a message from a `GET` request, the other links will display a version of the following form for sending the message:

![Web sample send message form](send-message-form.png "Web sample send message form")

Changing the number in the text box from even to odd numbers changes the response from the `Server` which can be observed in console output.

The web page renders synchronously; from the user's perspective, the interaction is synchronous and blocking, even though behind the scenes NServiceBus is implementing an asynchronous send-reply pattern using the [callbacks package](/nservicebus/messaging/callbacks.md).

### Initializing NServiceBus

In `WebApp`, open `Program.cs` and look at the code in the `UseNServiceBus` method:

snippet: ApplicationStart

The `builder.UseNServiceBus(endpointConfiguration)` call configures the web application to start an NServiceBus endpoint and registers an instance of `IMessageSession` which will be used to send messages outside of a message handler context (i. e. from MVC controllers, Razor Pages, and Blazor components).





### Sending a message

Open `Index.cshtml.cs` in `WebApp` to see the `OnPostAsync` method:

snippet: ActionHandling

The first line of code parses the text passed in by the user. The second line creates a new NServiceBus message of the type `Command`, and initializes its `Id` property with the value from the text box.

Open the class definition for the `Command` type in the `Shared` project:

snippet: Message

Return to `Index.cshtml.cs` and look at the code `messageSession.Request`. The message session offers methods to send messages via NServiceBus. Skip the rest of the code and see what happens to the message just sent.

### Handling the message

In the `Server` project, find this code in the `CommandMessageHandler` class:

snippet: Handler

This class implements the NServiceBus interface `IHandleMessages<T>` where `T` is the specific message type being handled; in this case, the `Command` message. NServiceBus manages classes that implement this interface. When a message arrives in the input queue, it is deserialized and then, based on its type, NServiceBus instantiates the relevant message handler classes and calls their `Handle` method, passing in the message object.

In the method body notice the response being returned to the originating endpoint. This will result in a message being added to the input queue for `MyWebClient` endpoint.


## Handling the response

When the response arrives back at `WebApp`, NServiceBus invokes the callback that was registered when the request was sent.

The `IMessageSession.Request` method takes the callback code and tells NServiceBus to invoke it when the response is received. There are several overloads of this method; the code above accepts a generic `Enum` parameter, effectively casting the return code from the server to the given enumeration type.

Finally, the code updates the `Text` property of a label on the web page, setting it to the string that represents the enumeration value: sometimes `None`, sometimes `Fail`.
