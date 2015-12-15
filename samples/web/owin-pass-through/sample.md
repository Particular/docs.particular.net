---
title: OWIN HTTP Message Pass Through
summary: Illustrates how to hook into the low level OWIN pipeline to place message onto the bus or directly onto the queue
tags:
related:
 - samples/gateway
 - nservicebus/gateway
---

## Introduction

This sample leverages OWIN (Open Web Interface for .NET) to add light weight HTTP message pass through middleware (general term for an extension to OWIN) that can be re-used in a variety of web technologies. This middleware provides the bridge between a HTTP stream (via JavaScript on a web page) and the queue used by NServiceBus. 

The flow of this samples is as follows

 * User performs some action on a webpage that triggers some JavaScript
 * JavaScript posts the message body to a specific URL
 * OWIN intercepts that post and passes to the bus middleware
 * The middleware takes the HTTP request, optionally deserializes it, and places it on the queue  


## What is [OWIN](http://owin.org/)

> OWIN defines a standard interface between .NET web servers and web applications. The goal of the OWIN interface is to decouple server and application, encourage the development of simple modules for .NET web development, and, by being an open standard, stimulate the open source ecosystem of .NET web development tools.

So extensions to NServiceBus that plug into OWIN can be easily applied to [many .NET web server technologies](http://owin.org/#projects).


## The purpose of this sample 

The primary purpose of this sample is to illustrate how simple it is to bridge the world of HTTP with the world of a service bus. The secondary purpose is to illustrate, as well as compare and contrast, two ways of communicating with the NServiceBus. i.e. using the Bus api and using the native queue.


## Comparisons with the [NServiceBus Gateway](/nservicebus/gateway)


### Performance

 * The Gateway uses a [HttpListener](https://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx) while this sample allows you to leverage the full power of your choice of webserver.
 * The Gateway is limited to run in a single endpoint while this samples allows you to use any well known web scale out technologies. 


### Full Control of the incoming message

This sample allow you to customize the http-to-message handling code that places the message on the queue. As such this allows you to 

 * Write custom validation rules on the message 
 * Return custom errors to the HTTP client
 * Apply custom authentication and authorization
 * Perform custom serialization


### Hosting 

The gateway is designed to run inside a NServiceBus endpoint. This sample code can run with your selection of technologies e.g. it will work with IIS, self-hosted, asp.mvc, NancyFX or within a NServiceBus endpoint in the same way as the Gateway.


### Cross Site effects

When using the gateway to perform a HTTP pass through the call will most likely be hosted on a different domain. As such a normal HTTP request will be blocked. To work around this you will need to do a [JSONP request](https://en.wikipedia.org/wiki/JSONP).

With the OWIN approach you have full control over the HTTP pipeline and hence can leverage CORS to avoid the need for JSONP.


## CORS

For layout and simplicity reasons this sample hosts the client side HTML/JavaScript parts in their own WebApplication. This means [CORS](https://en.wikipedia.org/wiki/Cross-origin_resource_sharing) need to be enabled on the HTTP server hosted in the endpoint. This is done using another OWIN Middleware from the Katana project called [Microsoft.Owin.Cors](https://www.nuget.org/packages/Microsoft.Owin.Cors/).

Note: CORS is enabled for all via `builder.UseCors(CorsOptions.AllowAll);`. You most likely want to change this in any real world usage.


## WebServer/OWIN Hosting

This sample uses a [Self Hosted](http://katanaproject.codeplex.com/wikipage?title=Selfhosting) instance of [Katana](http://www.asp.net/aspnet/overview/owin-and-katana) to serve up HTTP and provide an OWIN pipeline.


## The Endpoint Configuration

The endpoint configuration is fairly standard. The one exception is that the instance of `IBus` is passed in to the OWIN configuration code. 

snippet:startbus


## HTTP Hosting and OWIN middleware

A self-hosted instance of Katana is started and then different middleware are injected into the OWIN pipeline. Note that they are wired to map to specific URL suffixes.

 * `/to-bus` for the Bus based interception
 * `/to-msmq` for the direct to MSMQ interception

snippet:startowin


## Bus based middleware

The Bus based approach takes the following steps

 * Reads text for the message body from the HTTP request
 * Reads the message type name from the required headers
 * Converts the message type name to a .NET Type
 * Uses Type and message body, in conjunction with Json.net, to deserialize an instance of the real message
 * Places that message on the bus via a `SendLocal` 

snippet:OwinToBus


## MSMQ based middleware

The MSMQ based approach takes the following steps

 * Reads text for the message body from the HTTP request
 * Reads the message type name from the required headers
 * Uses the message type name to create a MSMQ transport compatible header string.
 * Places that body and header directly onto MSMQ
 
snippet:OwinToMsmq


### Header Helper

A helper method for creating an header string that is compatible with the NServiceBus MSMQ transport

snippet:msmqheaderserializer


## Comparing the two approaches

|| Bus Based | Native MSMQ                                                                                                                                                          
|-|-|-|
| Code Complexity         | Simple.                                                                                                    | Slightly more complicated since knowledge of the transport is required.                                                                                              |
| Performance             | Slightly slower and uses more memory since every message needs to be deserialized before being sent to the Bus. | Slightly faster and less memory since no deserialization or translation needs to occur.                                                                              |
| Transport compatibility | Works with any NServiceBus transport.                                                                      | Requires some native code for each transport.                                                                                                                        |
| Serialization errors    | Errors in deserialization will result in the request failing and an error being returned to the client.    | Errors in deserialization will occur when the bus attempts to process the message and hence will leverage the built in error handling functionality of NServiceBus. |


## JavaScript HTTP Post

snippet:PostMessage
