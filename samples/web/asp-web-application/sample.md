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

This sample shows how to send messages from an ASP.NET Core web application to an NServiceBus endpoint using several ASP.NET frameworks:
- MVC
- Web API
- Razor Pages
- Blazor

## Run the sample

There are three projects in the solution:
- `Server` - A console application which hosts the NServiceBus endpoint that handles messages sent from the `WebApp` project
- `WebApp` - An ASP.NET web application that sends messages to the `Server` endpoint using the frameworks listed above
- `Shared` - A library which contains the message definition, shared by both the `Server` and `WebApp` projects

Both the `Server` and `WebApp` projects must be running.  When the `WebApp` is run, a browser window will open to display links for sending messages using different technologies.

Excluding the Web API link, which sends a message from a `GET` request, the other links will display a version of the following form for sending the message using the specified framework:

![Web sample send message form](send-message-form.png "Web sample send message form")

Changing the number in the text box from even to odd numbers changes the response from the `Server` which can be observed in console output, as well as on the web page.

The web page renders synchronously; from the user's perspective, the interaction is synchronous and blocking, even though behind the scenes NServiceBus is implementing an asynchronous send-reply pattern using the [callbacks package](/nservicebus/messaging/callbacks.md).

### Initializing NServiceBus

In the `WebApp` project, open `Program.cs` and look at the endpoint configuration:

snippet: ApplicationStart

The `transport.RouteToEndpoint(typeof(Command), "Samples.Web.Server");` line informs the transport that all messages of type `Command` should be routed to the `Samples.Web.Server` endpoint. This enables sending of the message without having to specify the destination explicitly  when sending.

The `builder.UseNServiceBus(endpointConfiguration);` line configures the web application to start an NServiceBus endpoint and registers an instance of `IMessageSession` for dependency injection. 

### Sending a message

Regardless of the framework used, a message is sent using an [injected](/nservicebus/hosting/asp-net.md#dependency-injection) instance of `IMessageSession`. This is an API used to send messages outside of the NServiceBus message handling pipeline (i. e. from MVC controllers, Razor Pages, and Blazor components).

Each framework example uses `IMessageSession.Request` to send the following message and asynchronously wait for the response from the `Server` handler using the [callbacks package](/nservicebus/messaging/callbacks.md):

snippet: Message

> [!NOTE]
> The basic steps of sending a message using ASP.NET are the same:
> 1. Inject `IMessageSession`
> 2. Use `IMessageSession` to send a message
> 
> Reference the framework example most relevant to your needs.

#### MVC

The MVC implementation can be found at `WebApp/Controllers/SampleController.cs`:

snippet: MVCSendMessage

#### Web API

The Web API implementation can be found at `WebApp/Api/SampleApiController.cs`:

snippet: WebApiSendMessage

#### Razor Pages

The Razor Pages implementation can be found at `WebApp/Pages/SendMessageRazorPages.cshtml.cs`:

snippet: RazorPagesSendMessage

#### Blazor

The Blazor implementation can be found at `WebApp/Pages/Shared/SendMessageBlazor.razor`:

snippet: BlazorSendMessage

> [!NOTE]
> There is a `Blazor` action in the MVC controller that is used to render the Blazor component used in the example.

### Handling the message

In the `Server` project, the `CommandMessageHandler` class handles the message that is sent from the `WebApp`:

snippet: Handler

This class implements the NServiceBus interface `IHandleMessages<T>` where `T` is the specific message type being handled; in this case, the `Command` message. When a message arrives in the input queue, it is deserialized and then, based on its type, NServiceBus instantiates the relevant message handler classes and calls their `Handle` method, passing in the message object.

In the method body notice the [reply](/nservicebus/messaging/reply-to-a-message.md) to the originating endpoint. This will result in a message being added to the input queue for the endpoint that sent the message, in this case, the `Samples.Web.WebApplication` endpoint.

## Handling the response

When the reply message arrives at the `WebApp` endpoint, NServiceBus invokes the callback that was registered when the request was sent.

The `IMessageSession.Request` method takes the callback code and tells NServiceBus to invoke it when the response is received. There are several overloads of this method; the code above accepts a generic `Enum` parameter, effectively casting the return code from the server to the given enumeration type.

Finally, the code updates the `Text` property of a label on the web page, setting it to the string that represents the enumeration value: sometimes `None`, sometimes `Fail`.
