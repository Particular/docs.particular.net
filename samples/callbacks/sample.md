---
title: Callback Sample
summary: Illustrates the callback behavior of NServiceBus.
tags:
- Callback
related:
- nservicebus/messaging/handling-responses-on-the-client-side
---

 1. Run the solution. Two console applications start.
 2. In the Sender application you will be prompted to press various keys to trigger each scenario. 


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

## Sender project

A console application responsible for sending a messages and handling the callback from the reply.


### Send and callback for an enum

<!-- import SendEnumMessage -->


### Send and callback for an int

<!-- import SendIntMessage -->


### Send and callback for an object

<!-- import SendObjectMessage -->

Note: In Version 3 if no handler exists for a received message then NServiceBus will throw an exception. As such for this scenario to operate a fake message handler is needed on the callback side.

<!-- import ObjectResponseMessageHandler -->


## Receiver project

A console application responsible replying to messages.


### Return an enum

<!-- import EnumMessageHandler -->


### Return an int 

<!-- import IntMessageHandler -->


### Return an Object

Note that this scenario requires a `Reply` with a real message.

<!-- import ObjectMessageHandler -->