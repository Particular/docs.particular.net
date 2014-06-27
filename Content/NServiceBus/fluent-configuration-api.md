---
title: Fluent Configuration API
summary: Introduction to NServiceBus Fluent Configuration API
tags:
- NServiceBus
- Fluent Configuration
---

Every NServiceBus endpoint to work properly relies on a configuration that determines settings and behaviors other than endpoint core functionalities.

###NServiceBus Host

NServiceBus provides its own hosting service that can be used to outsource the whole hosting "drama" (find a synonym) without worrying about how to deal with Windows Services.

When using the built in hosting services the endpoint configuration is specified using the EndpointConfig class, automatically created when adding NServiceBus packages via NuGet, and implementing one of the core interfaces that determine the default endpoint behavior:

* *As_AServer*: Indicates this endpoint is a server.  As such will be set up as a transactional endpoint using impersonation, not purging messages on startup.
* *As_APublisher*: extends "as a server" Indicates this endpoint is a publisher.  This is compatible with NServiceBus.AsA_Server but not NServiceBus.AsA_Client.
* *As_AClient*: Indicates this endpoint is a client.  As such will be set up as a non-transactional endpoint with no impersonation and purging messages on startup.

Sample class

Explain the meaning of IConfigureThisEndpoint

###Configuration customization

When NServiceBus endpoints are hosted using the built in NServiceBus host it is possible to customize the default configuration adding to the project a class that implements the IWantCustomInitialization interface, this class will be invoked at runtime by the hosting process and will be provided with a default configuration initialized by the host and ready to be configured as required.

Sample class

NOTE: Do not start the bus it will be done by the host

###features

NServiceBus has also the notion of features, features are a high level concept that encapsulate a set of settings related to a certain feature, thus features can be entirely enabled or disabled and when enabled configured.

List of built-in features

To enable or disable a feature there is a simple and straightforward API:

Configure.Features.Enable<Sagas>();

The above sample enables Sagas support in the endpoint where is executed.

###Self-hosting configuration

There are scenarios in which we need to self host the bus without being able to rely on the NServiceBus Host process, a well known sample scenario is a web application, in these cases we are responsible to configure, create and start the bus.
The configuration entry point point is the Configure class that exposes a fluent configuration API that let us take fine control of all the configuration settings.


###Fluent Configuration API