---
title: OWIN Pass Through
summary: Illustrates how to hook into the low level OWIN pipeline to place message onto the bus or directly onto the queue
tags:
related:
---

## What is [OWIN](http://owin.org/)

> OWIN defines a standard interface between .NET web servers and web applications. The goal of the OWIN interface is to decouple server and application, encourage the development of simple modules for .NET web development, and, by being an open standard, stimulate the open source ecosystem of .NET web development tools.

So extensions to NServiceBus that plug into OWIN can be easily applied to [many .net web server technologies](http://owin.org/#projects).


## WebServer/OWIN Hosting

This sample uses a [Self Hosted](http://katanaproject.codeplex.com/wikipage?title=Selfhosting) instance of [Katana](http://www.asp.net/aspnet/overview/owin-and-katana) to serve up HTTP and provide an OWIN pipeline.


## The Endpoint Configuration

The endpoint configuration is fairly standard. The one exception is that the instance of `IBus` is passed in to the OWIN configuration code. 

<!-- import startbus -->


## HTTP Hosting and OWIN middleware

A self-hosted instance of Katana is started and then different middleware are injected into the OWIN pipeline. Note that they are wired to map to specific URL suffixes.

 * `/to-bus` for the Bus based interception
 * `/to-msmq` for the direct to MSMQ interception

<!-- import startowin -->


## Bus based middleware

The Bus based approach takes the following steps

 * Reads text for the message body from the HTTP request
 * Reads the message type name from the requires headers
 * Converts the message type name to a .net Type
 * Uses Type and message body, in conjunction with Json.net, to deserialize an instance of the real message
 * Places that message on the bus via a `SendLocal` 

<!-- import OwinToBus -->


## MSMQ based middleware

The MSMQ based approach takes the following steps

 * Reads text for the message body from the HTTP request
 * Reads the message type name from the requires headers
 * Uses the message type name to create a MSMQ transport compatible header string.
 * Places that body and header directly onto MSMQ
 
<!-- import OwinToMsmq -->


### Header Helper

A helper method for creating an header string that is compatible with the NServiceBus MSMQ transport

<!-- import MsmqHeaderSerializer -->


## Comparing the two approaches


|| Bus Based | Native MSMQ                                                                                                                                                          
|-|-|-|
| Code Complexity         | Simple.                                                                                                    | Slightly more complicated since knowledge of the transport is required.                                                                                              |
| Performance             | Slightly slower and uses more memory since every message needs to be deserialized before be send to the Bus. | Slightly faster and less memory since no deserialization or translation needs to occur.                                                                              |
| Transport compatibility | Works with any NServiceBus transport.                                                                      | Requires some native code for each transport.                                                                                                                        |
| Serialization errors    | Errors in deserialization will result in the request failing and an error being returned to the client.    | Errors in deserialization will occur when the bus attempts to process the message and hence will leverage the built in error handling functionality of NServiceBus. |


## JavaScript HTTP Post

<!-- import PostMessage -->


## CORS

For layout and simplicity reasons this sample hosts the client side HTML/JavaScript parts in their own WebApplication. This means [CORS](https://en.wikipedia.org/wiki/Cross-origin_resource_sharing) need to be enabled on the HTTP server hosted in the endpoint. This is done using another OWIN Middleware from the Katana project called [Microsoft.Owin.Cors](https://www.nuget.org/packages/Microsoft.Owin.Cors/).

Note: CORS is enabled for all via `builder.UseCors(CorsOptions.AllowAll);`. You most likely want to change this in any real world usage.
