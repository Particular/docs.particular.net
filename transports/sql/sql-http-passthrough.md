---
title: SQL HTTP Passthrough
summary: Place a raw HTTP request directly onto the SQL Server transport.
reviewed: 2018-05-20
component: SqlHttpPassthrough
related:
 - samples/web/sql-http-passthrough
---

SQL HTTP Passthrough provides a bridge between an HTTP stream (via JavaScript on a web page) and the [SQL Server Transport](/transports/sql/). It leverages [SQL Transport - Native](/transports/sql/sql-native.md) and [SQL Attachments](/nservicebus/messaging/attachments-sql.md).


## Design


### Server side hosting

SQL HTTP Passthrough is designed to be consumed by any web application built on [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/). So, for example, it can be used to send a message from a [Controller](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/actions), a [BaseController](https://coderwall.com/p/cibprg/basecontroller-in-asp-net-mvc), a [Filter](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters) or a [Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/).


### Deduplication

To handle intermittent connectivity issues it is desirable to have a web client leverage a retry mechanism. So if a request fails, the same request can be immediately re-sent. To prevent this resulting in duplicate message being placed on the queue, message deduplication has to occur. SQL HTTP Passthrough leverages the [deduplication feature](/transports/sql/sql-native.md#Deduplication) of SQL Transport - Native.


### Data and Attachments

To send both a message content and associated binary data (attachments) a [multipart form](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Disposition) is used.


## Usage


### Server Side


#### ASP.NET Core Startup

At [ASP.NET Core Startup](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) several actions are taken.

 * `AddSqlHttpPassthrough` is called on [IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) which makes the `ISqlPassthrough` interface available via [dependency injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).
 * `AddSqlHttpPassthroughBadRequestMiddleware` is called on [IApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder), which adds [Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/) to the pipeline. This means that if the request parsing code of the SQL HTTP Passthrough throws a `BadRequestException`, that exception can be gracefully handled and a [HTTP BadRequest (400)](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400) can be sent as a response. This is optional, and a Controller can choose to explicitly catch and handle `BadRequestException` in a different way.

snippet: Startup


##### Message Callback

`AddSqlHttpPassthrough` takes a required parameter `callback` with the signature `Func<HttpContext, PassthroughMessage, Task<Table>>`. This delegate will be called during each request-to-message execution. This occurs after the HTTP request has been parsed, and before the outgoing message is placed on the SQL table. The return value is a `Table` that dictates the SQL table and schema that the message will be written to.

While callback supports async, via returning a `Task<Table>`, any required async action should have its result cached so as to not slow down subsequent requests. For example, it may be necessary to perform some kind of authorization in a callback. The result of this authorization should be cached for some period of time, and the cached result should be purged when permissions are changed.

The message callback can be used for several purposes:

* Validate that the message type and destination are allowed.
* Add extra headers to the outgoing message.
* Manipulate any other properties of the outgoing message

WARNING: Note that a "trust but verify" approach should be take in regards to the HTTP client. The combination of message type/namespace and destination should be verified against a known allowed list.


`PassthroughMessage` contains the following properties:

 * Id: Contains the `MessageId` value from `HttpRequest.Headers`
 * CorrelationId: Contains the `MessageId` value from `HttpRequest.Headers`
 * Type: Contains the `MessageType` value from `HttpRequest.Headers`. Will be combined with `Namespace` and used for the `NServiceBus.EnclosedMessageTypes` header.
 * Namespace: Contains the `MessageNamespace` value from `HttpRequest.Headers`. Will be combined with `Type` and used for the `NServiceBus.EnclosedMessageTypes` header.
 * Body: Contains the `Message` value from the `IFormCollection`.
 * Destination: Contains the 'Destination' value from `HttpRequest.Headers`. Primarily used to convert to a `Table` as a return value for the passthrough callback.
 * ClientUrl: The URL of the submitting page. Contains the `HeaderNames.Referer` value from `HttpRequest.Headers`. Will be written to a header `MessagePassthrough.ClientUrl` in the outgoing NServiceBus message.
 * Attachments: Contains all binaries extracted from `IFormCollection.Files`
 * ExtraHeaders: Any extra headers to add to the outgoing NServiceBus message.


#### Usage in a Controller

Usage in a [Controller](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/actions) consists of several parts.

 * `ISqlPassthrough` injected through [dependency injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).
 * The controller handling the HTTP Post and passing that information to `ISqlPassthrough.Send`.

snippet: Controller

WARNING: In a production application the Controller would be performing any authorization and authentication on the incoming request. 


#### Exception behavior

If `ISqlPassthrough` fails to send, a `SendFailureException` will be thrown containing all context in a `PassthroughMessage` property.

If the incoming HTTP request fails to be parsed, a `BadRequestException` will be thrown with the message containing the reason for the failure.


### Client - JavaScript 


#### Form submission

The JavaScript that submits the data does so through by building up a [FormData](https://developer.mozilla.org/en-US/docs/Web/API/FormData) and [Posting](https://developer.mozilla.org/en-US/docs/Learn/HTML/Forms/Sending_and_retrieving_form_data#The_POST_method) that via the [Fetch API](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API).

snippet: PostToBus


#### MessageId generation

For deduplication to operate, the client must generate a [MessageId](/nservicebus/messaging/message-identity.md), so that any retries can be ignore. JavaScript does not contain native functionality to generate a GUID, so a helper method is used. 

snippet: Guid


### Client .NET

Creating and posting a multipart form can be done using a combination of [MultipartFormDataContent](https://msdn.microsoft.com/en-us/library/system.net.http.multipartformdatacontent.aspx) and [HttpClient.PostAsync](https://msdn.microsoft.com/en-us/library/system.net.http.httpclient.postasync.aspx). To simplify this action the `ClientFormSender` class can be used:

snippet: ClientFormSender

This can be useful when performing [Integration testing in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing).

snippet: asptesthost
