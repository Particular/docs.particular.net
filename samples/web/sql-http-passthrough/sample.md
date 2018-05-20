---
title: SqlHttpPassthrough
summary: Place a raw HTTP request directly onto the SQL Server transport.
reviewed: 2018-05-20
component: SqlHttpPassthrough
related:
 - samples/web/owin-pass-through
---

This sample leverages the [SQL Server - HTTP Passthrough](/transports/sql/sql-http-passthrough.md) to provide a bridge between an HTTP stream (via JavaScript on a web page) and the SQL Transport queue used by NServiceBus.

The flow of this sample is:

 * User performs some action on a webpage that triggers some JavaScript
 * JavaScript posts the message body to a specific URL
 * Controller takes the HTTP request and places it in a queue


## Prerequisites

include: sql-prereq

The database created by this sample is `SqlHttpPassThroughSample`.


## Running the sample

When the solution is started two projects will start:

 * Endpoint
 * Web (As a console and browser)

In the browser press the button and a message will be received by the Endpoint.


## Code walk-through


### Endpoint

This is a standard NServiceBus endpoint that will receive the message.


#### EndpointConfiguration

The endpoint is configured as follows:

 * Use the [SQL Server Transport](/transports/sql).
 * Use the [Newtonsoft JSON serializer](/nservicebus/serialization/newtonsoft.md). The choice of serializer is important since that format will need to be consistent when sending in the web context.
 * Use [SQL Attachments](/nservicebus/messaging/attachments-sql.md) for processing the binaries sent through HTTP.

snippet: EndpointConfiguration


#### Handler

The receiving handler outputs all incoming headers and attachments.

snippet: Handler


#### Message contract

There is a single message with two properties to illustrate the data being passed through 

snippet: MessageContract

Note that the messages exist only in this endpoint and do not need to be used, via a reference, in the Web project.


### Web


#### Startup

At [ASP.NET Core Startup](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup) several actions are taken.

 * `AddSqlHttpPassThrough` is called on [IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection) which makes the `ISqlPassThrough` interface available via [dependency injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).
 * `AddSqlHttpPassThroughBadRequestMiddleware` is called on [IApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder), which adds [Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/) to the pipeline. This means that if the request parsing code of the SQL HTTP Passthrough throws a `BadRequestException`, that exception can be gracefully handled and a [HTTP BadRequest (400)](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400) can be sent as a response. This is optional, and a Controller can choose to explicitly catch and handle `BadRequestException` in a different way.

snippet: Startup


#### PassThroughController

The `PassThroughController` consists of several parts.

 * `ISqlPassThrough` injected through [dependency injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).
 * The controller handling the HTTP Post and passing that information to `ISqlPassThrough.Send`.

snippet: PassThroughController

WARNING: The controller that, in a production application, would be performing any authorization and authentication on the incoming request. 


#### SampleClientController

The `SampleClientController` serves up the HTML UI for this sample.

snippet: SampleClientController


#### HTML form

The HTML captures that data that will be submitted to the Passthrough controller.

snippet: form


#### Form submission

The JavaScript that submits the data does so through by building up a [FormData](https://developer.mozilla.org/en-US/docs/Web/API/FormData) and [Posting](https://developer.mozilla.org/en-US/docs/Learn/HTML/Forms/Sending_and_retrieving_form_data#The_POST_method) that via the [Fetch API](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API).

snippet: PostToBus


#### MessageId generation

For deduplication to operate, the client must generate a [MessageId](/nservicebus/messaging/message-identity.md), so that any retries can be ignore. JavaScript does not contain native functionality to generate a GUID, so a helper method is used. 

snippet: Guid