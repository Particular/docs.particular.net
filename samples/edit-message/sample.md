---
title: Editing message body
summary: Demonstrates how failing message can be modified and retried.
component: Service Pulse
reviewed: 2018-10-09
---


 1. Run the solution. Three console applications will start: Publisher, Subscriber and EndpointThatModifyTheMessage.
 1. In the Publisher console window click 1 to send a command.
 1. Subscriber will receive the message and fail while trying to process it.
 1. Now in ServicePulse navigate towards Configuration screen and add Redirect from: Samples.EditMessage.Subscriber@machine-name to Samples.EditMessage.EndpointThatModifyTheMessage@machine-name . [More information about creating Redirects](/servicepulse/redirect.md)
 1. From Service Pulse retry failed messages that were send to Subscriber. Those messages will be processed and modified by EndpointThatModifyTheMessage and send to Subscriber endpoint.


## Shared project

A class library containing the messages and shared code.


### Status enum

This is used for the enum callback scenario.

```
public enum Status
{
    OK,
    Error
}
```

include: callbacks-disclaimer

## WebSender project

An ASP.NET MVC application responsible for sending messages and handling the web callback from the reply. Depending upon the NServiceBus version, the method for integrating with asynchronous controllers is very different.

partial: static

partial: dotnetcore
