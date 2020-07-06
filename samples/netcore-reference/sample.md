---
title: Building endpoints with .NET Core 3.1
summary: Hosting endpoints with the Generic Host.
reviewed: 2020-07-06
component: Core
related:
- nservicebus/hosting/extensions-hosting
- nservicebus/dependency-injection/extensions-dependencyinjection
---

This sample implements a very simple application running on .NET Core 3.1. It shows how to integrate NServiceBus with an ASP.NET Core Web API project and a simple back-end process running in the .NET Core generic host. It also demonstrates using the built-in tools to inject dependencies into message handler classes.

## Front-end

The front-end project hosts a simple web page that contains a form. When the form is submitted, a message is sent to the back-end application via NServiceBus for processing. 

The front-end uses the .NET Core generic host to host an NServiceBus endpoint and ASP.NET Core WebAPI controllers. The endpoint is configured as a [send only](/nservicebus/hosting/) endpoint as it does not need to process any messages. There is routing configured for a single message type: `SomeMessage` messages get routed to `Sample.BackEnd`.

snippet: front-end-wire-up

The WebAPI controller is able to take a dependency on `IMessageSession` and use it to send messages to the backend.

snippet: front-end-controller


## Back-end

The back-end project hosts an NServiceBus endpoint which processes messages sent from the front-end. When handling each message it relies on a service that is registered with the host's service collection.

The back-end uses the same API as the front-end to register an NServiceBus endpoint with the generic host.

snippet: back-end-use-nservicebus

Additionally, it registers an important calculation service with the host's service collection.

snippet: back-end-register-service

The NServiceBus message handler is able to take a dependency on the registered service and use it when handling the message.

snippet: back-end-handler