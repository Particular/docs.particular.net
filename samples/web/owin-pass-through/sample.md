---
title: OWIN HTTP Message Pass Through
summary: How to hook into the OWIN pipeline to place a message onto the bus or directly onto the queue
reviewed: 2018-02-26
component: Core
related:
 - samples/gateway
 - nservicebus/gateway
---


This sample leverages OWIN (Open Web Interface for .NET) to add light weight HTTP message pass through middleware (general term for an extension to OWIN) that can be re-used in a variety of web technologies. This middleware provides the bridge between an HTTP stream (via JavaScript on a web page) and the queue used by NServiceBus.

The flow of this sample is:

 * User performs some action on a webpage that triggers some JavaScript
 * JavaScript posts the message body to a specific URL
 * OWIN intercepts that post and passes the data to the middleware
 * The middleware takes the HTTP request, optionally deserializes it, and places it in a queue


## What is [OWIN](http://owin.org/)

> OWIN defines a standard interface between .NET web servers and web applications. The goal of the OWIN interface is to decouple server and application, encourage the development of simple modules for .NET web development, and, by being an open standard, stimulate the open source ecosystem of .NET web development tools.

Extensions to NServiceBus that plug into OWIN can be applied easily to [many .NET web server technologies](http://owin.org/#projects).


## The purpose of this sample

This sample illustrates how simple it is to bridge the world of HTTP with the world of a service bus. The secondary purpose is to illustrate two different ways of communicating with the NServiceBus. I.e. using the NServiceBus API and using the native queue.


## Comparisons with the [NServiceBus gateway](/nservicebus/gateway)

### Performance

 * The gateway uses an [HttpListener](https://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx) while this sample allows leveraging the full power of the choice of webserver.
 * The gateway is limited to run in a single endpoint while this sample supports the use of any well-known web scale-out technology.


### Full control of the incoming message

This sample allow the customization of the http-to-message handling code that places the message on the queue. This allows:

 * Writing custom validation rules on the message
 * Returning custom errors to the HTTP client
 * Custom authentication and authorization
 * Custom serialization


### Hosting

The gateway is designed to run inside an NServiceBus endpoint. The sample code can run with different technologies (e.g. IIS, self-hosted, asp.mvc, NancyFX or within an NServiceBus endpoint) in the same way as the gateway.


### Cross-site effects

When using the gateway to perform a HTTP pass through, the call will most likely be hosted on a different domain. As such, a normal HTTP request will be blocked. To work around this, a [JSONP request](https://en.wikipedia.org/wiki/JSONP) is required.

With the OWIN approach, full control over the HTTP pipeline is possible and can leverage CORS to avoid the need for JSONP.


## CORS

For simplicity, this sample hosts the client-side HTML/JavaScript parts in their own WebApplication. This means [CORS](https://en.wikipedia.org/wiki/Cross-origin_resource_sharing) must be enabled on the HTTP server hosted in the endpoint. This is done using another OWIN middleware from the Katana project called [Microsoft.Owin.Cors](https://www.nuget.org/packages/Microsoft.Owin.Cors/).

Note: CORS is enabled via `builder.UseCors(CorsOptions.AllowAll);`. In most cases, this should be changed for real world usage.


## WebServer/OWIN Hosting

This sample uses a [self hosted](https://katanaproject.codeplex.com/wikipage?title=Selfhosting) instance of [Katana](https://www.asp.net/aspnet/overview/owin-and-katana) to serve up HTTP and provide an OWIN pipeline.


## The endpoint configuration

The endpoint configuration is standard except that the endpoint instance is passed in to the OWIN configuration code.

snippet: startbus


## HTTP hosting and OWIN middleware

A self-hosted instance of Katana is started and then different middleware are injected into the OWIN pipeline. Note that they are wired to map to specific URL suffixes.

 * `/to-bus` for the bus-based interception
 * `/to-msmq` for the direct-to-MSMQ interception

snippet: startowin


## Bus-based middleware

The bus-based approach takes the following steps:

 * Reads text for the message body from the HTTP request
 * Reads the message type name from the required headers
 * Converts the message type name to a .NET Type
 * Uses type and message body, in conjunction with Json.net, to deserialize an instance of the real message
 * Places that message on the bus via `SendLocal`

snippet: OwinToBus


## MSMQ-based middleware

The MSMQ-based approach takes the following steps:

 * Reads text for the message body from the HTTP request.
 * Reads the message type name from the required headers.
 * Uses the message type name to create a MSMQ transport compatible header string.
 * Places that body and header directly onto MSMQ.

snippet: OwinToMsmq


### Header helper

A helper method for creating a header string that is compatible with the NServiceBus MSMQ transport:

snippet: msmqheaderserializer


## Comparing the two approaches

|| Bus-based | Native MSMQ
|-|-|-|
| Code Complexity | Simple | Slightly more complicated; knowledge of the transport is required |
| Performance | Slightly slower and uses more memory; every message needs to be deserialized before being sent to the bus | Slightly faster and less memory; no deserialization or translation needs to occur |
| Transport compatibility | Works with any NServiceBus transport | Requires some native code for each transport |
| Serialization errors | Deserialization errors will result in the request failing and an error being returned to the client | Deserialization errors will occur when the bus attempts to process the message and will leverage the built-in error handling functionality of NServiceBus |


## JavaScript HTTP Post

snippet: PostMessage