---
title: Transitioning to the MSMQ Transport
summary: Demonstrates the required changes to switch your POC from the Learning Transport to the MSMQ Transport
reviewed: 2019-04-24
component: MsmqTransport
tags:
 - MSMQ
 - Transport
related:
 - transports/msmq
 - samples/msmq/simple
---

The Learning transport is not a production-ready transport, but is intended for learning the NServiceBus API and creating demos/proof-of-concepts. A number of changes to endpoint configuration are required to transition an endpoint from the Learning Transport to the MSMQ Transport.


## Install the NuGet Package

The [NServiceBus.Transport.Msmq](https://www.nuget.org/packages/NServiceBus.Transport.Msmq/) must be installed. This can be accomplished using the [NuGet Package Manager UI](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui) or run this command from the [NuGet Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console):

```
   Install-Package NServiceBus.Transport.Msmq
```


## Change the Endpoint Configuration

To use the MSMQ Transport first update the `UseTransport` call:

```c#
-   endpointConfiguration.UseTransport<LearningTransport>();
+   endpointConfiguration.UseTransport<MsmqTransport>();
```


### Configure an Error Queue

An error queue (`error`) must [be specified](/nservicebus/recoverability/configure-error-handling), which was not a required setting in the learning transport. This is important because most MSMQ systems are distributed, and a centralized error queue in the form of `error@MACHINENAME` is used for the whole system.

snippet: ErrorWithCode


### Enable Installers

The endpoint must enable installers to allow the MSMQ Transport to create the necessary [queues](https://msdn.microsoft.com/en-us/library/ms705002.aspx) for your endpoint.

snippet: Installers


### Configure a Persistence

This is because the MSMQ Transport, unlike the Learning Transport, does not natively support Publish/Subscribe and instead uses [message-driven Pub/Sub](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based), so the message subscription information must be stored.

snippet: ConfiguringMsmqPersistence

The MSMQ transport provides a [subscription storage persistence](/persistence/msmq/subscription). Use the [selecting a persister](/persistence/selecting) guide to help determine if MSMQ persistence is appropriate for the endpoint.


include: registerpublishers
