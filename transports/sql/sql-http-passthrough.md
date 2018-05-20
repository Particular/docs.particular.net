---
title: SQL HTTP Passthrough
summary: Place a raw HTTP request directly onto the SQL Server transport.
reviewed: 2018-05-20
component: SqlHttpPassthrough
related:
 - samples/web/sql-http-passthrough
---

SQL HTTP Passthrough provide a bridge between an HTTP stream (via JavaScript on a web page) and the SQL Transport queue used by NServiceBus. It leverages [SQL Transport - Native](/transports/sql/sql-native.md) and [SQL Attachments](/nservicebus/messaging/attachments-sql.md).


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

 * `AddSqlHttpPassThrough` is called on [IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) which makes the `ISqlPassThrough` interface available via [dependency injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).
 * `AddSqlHttpPassThroughBadRequestMiddleware` is called on [IApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder), which adds [Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/) to the pipeline. This means that if the request parsing code of the SQL HTTP Passthrough throws a `BadRequestException`, that exception can be gracefully handled and a [HTTP BadRequest (400)](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400) can be sent as a response. This is optional, and a Controller can choose to explicitly catch and handle `BadRequestException` in a different way.

snippet: Startup


#### Usage in a Controller

Usage in a [Controller](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/actions) consists of several parts.

 * `ISqlPassThrough` injected through [dependency injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).
 * The controller handling the HTTP Post and passing that information to `ISqlPassThrough.Send`.

snippet: Controller

WARNING: In a production application the Controller would be performing any authorization and authentication on the incoming request. 


#### Exception behavior

If `ISqlPassThrough` fails to send a `SendFailureException` will be thrown containing all context in a `PassThroughMessage` property.

If a the incoming HTTP request fails to be parsed a `BadRequestException` will be thrown with the message containing the reason for the failure.


### JavaScript - Client 


#### Form submission

The JavaScript that submits the data does so through by building up a [FormData](https://developer.mozilla.org/en-US/docs/Web/API/FormData) and [Posting](https://developer.mozilla.org/en-US/docs/Learn/HTML/Forms/Sending_and_retrieving_form_data#The_POST_method) that via the [Fetch API](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API).

snippet: PostToBus


#### MessageId generation

For deduplication to operate, the client must generate a [MessageId](/nservicebus/messaging/message-identity.md), so that any retries can be ignore. JavaScript does not contain native functionality to generate a GUID, so a helper method is used. 

snippet: Guid