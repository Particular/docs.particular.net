---
title: Web application hosting
summary: Hosting NServiceBus in a website or web service
reviewed: 2025-01-14
isLearningPath: true
related:
 - samples/web
 - nservicebus/lifecycle
 - samples/startup-shutdown-sequence
 - nservicebus/messaging/callbacks
 - nservicebus/hosting/publishing-from-web-applications
---

NServiceBus can be integrated into any web technology that supports .NET.

As most web technologies operate in a scale-out manner, NServiceBus can be hosted in a "Send-only" configuration. In this mode, the web application technology acts as a "sender" of messages rather than the "processor". The handling code of a given web request leverages the NServiceBus send APIs and the messages are processed by a backend service.

## Dependency injection integration

### Using the Generic Host

NServiceBus can be integrated into any web host that supports the [Microsoft Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host) using the [NServiceBus Generic Host integration](/nservicebus/hosting/extensions-hosting.md). The Generic Host integration automatically registers `IMessageSession` with the dependency injection container.

### Self-hosting

Web request handlers require access to the endpoint messaging session to [send messages](/nservicebus/messaging/send-a-message.md) as a result of incoming HTTP requests. Many of the supported web application hosts resolve these web request handlers using dependency injection. NServiceBus already creates and manages its own dependency injection when using self-hosting.

The recommended approach to handle this scenario is to have two dependency injection instances: one for the web host and its web request handlers, and another for the NServiceBus endpoint and its message handlers. There are some things to consider when adopting this approach.

* Any service which is registered with the NServiceBus dependency injection can be injected into NServiceBus message handlers but not the web request handlers.
* Any service which is registered with the web host dependency injection mechanism can be injected into web request handlers but not the NServiceBus message handlers.
* Any service which is registered with both dependency injection instances can be resolved in both web request handlers and NServiceBus message bus handlers _but_:
  * These will be different object instances with different lifetimes.
  * Even if the services are registered with a singleton lifetime, there will still be one created for each dependency injection instance unless the instance is explicitly managed by the caller.
  * If a service must be shared and a single instance, it must be created externally during the web application host startup, and that specific instance must be registered in both dependency injection instances.

## Endpoint lifecycle

In a web-hosted scenario, [recycling an IIS process](https://docs.microsoft.com/en-us/previous-versions/iis/6.0-sdk/ms525803(v=vs.90)) causes the hosted NServiceBus endpoint to shutdown and restart.
