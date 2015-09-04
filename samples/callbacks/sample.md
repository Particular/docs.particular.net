---
title: Callback Sample
summary: Illustrates the callback behavior of NServiceBus.
tags:
- Callback
related:
- nservicebus/messaging/handling-responses-on-the-client-side
---

 1. Run the solution. Two console applications and a web application will start.
 2. In the WebSender application, you can click links from the landing page to trigger each scenario.
 3. In the Sender console application, you will be prompted to press various keys to trigger each scenario.


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


### Send and callback for an enum

<!-- import Web_SendEnumMessage -->


### Send and callback for an int

<!-- import Web_SendIntMessage -->


### Send and callback for an object

<!-- import Web_SendObjectMessage -->

Note: In Version 3 if no handler exists for a received message then NServiceBus will throw an exception. Because Version 3 does not log to a file by default, this can be very hard difficult to detect, but it is quite wasteful as the message may be retried for no reason. For this scenario to operate without wasting server resources, a fake message handler is needed on the callback side.

<!-- import Web_ObjectResponseMessageHandler -->

## Sender project

A console application responsible for sending a messages and handling the callback from the reply, as an alternative to a web application.


### Send and callback for an enum

<!-- import SendEnumMessage -->


### Send and callback for an int

<!-- import SendIntMessage -->


### Send and callback for an object

<!-- import SendObjectMessage -->

Note: In Version 3 if no handler exists for a received message then NServiceBus will throw an exception. As such for this scenario to operate a fake message handler is needed on the callback side.

<!-- import ObjectResponseMessageHandler -->


## Receiver project

A console application responsible replying to messages from either the web application or the console application.


### Return an enum

<!-- import EnumMessageHandler -->


### Return an int 

<!-- import IntMessageHandler -->


### Return an Object

Note that this scenario requires a `Reply` with a real message.

<!-- import ObjectMessageHandler -->