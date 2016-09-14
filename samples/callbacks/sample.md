---
title: Callback Usage
component: Callbacks
reviewed: 2016-03-21
tags:
- Callback
---

 1. Run the solution. Two console applications and a web application will start.
 1. In the WebSender application, click links from the landing page to trigger each scenario.
 1. In the Sender console application, when prompted to press various keys to trigger each scenario.


## Shared Project

A class library containing the messages and shared code.


### Status Enum

To be used for the enum callback scenario.

```
public enum Status
{
    OK,
    Error
}
```


## WebSender project

An ASP.NET MVC application responsible for sending messages and handling the web callback from the reply. Depending upon NServiceBus version, the method for integrating with asynchronous controllers is very different.

Note: All the usages of the Bus are done via a static instance stored on the root `MvcApplication` that is configured in the `Global.cs`.


### Send and callback for an enum

snippet:Web_SendEnumMessage


### Send and callback for an int

snippet:Web_SendIntMessage


### Send and callback for an object

snippet:Web_SendObjectMessage


## Sender project

A console application responsible for sending a messages and handling the callback from the reply, as an alternative to a web application.


### Send and callback for an enum

snippet:SendEnumMessage


### Send and callback for an int

snippet:SendIntMessage


### Send and callback for an object

snippet:SendObjectMessage


## Receiver project

A console application responsible replying to messages from either the web application or the console application.


### Return an enum

snippet:EnumMessageHandler


### Return an int

snippet:IntMessageHandler


### Return an Object

Note that this scenario requires a `Reply` with a real message.

snippet:ObjectMessageHandler