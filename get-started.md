---
title: Getting Started
summary: Quick list of instructions and hints to get started with NServiceBus and the Particular Service Platform
reviewed: 2016-12-05
---

There is a variety of starting points for NServiceBus and the Platform. The path taken will depend on an individual's specific experience and preferences for learning.


## High Level Content

These articles are recommended to gain an understanding of the key concepts and technologies used within the Platform.

 * [Service Platform Overview](/platform/): Provides an overview of the various parts of Platform; [NServiceBus](/nservicebus/), [ServiceControl](/servicecontrol/), [ServiceInsight](/serviceinsight/) and [ServicePulse](/servicepulse/).
 *  [Architectural Principles](/nservicebus/architecture/principles.md) and [Bus vs Broker architecture](/nservicebus/architecture/): A high-level overview of messaging architectures and the two popular approaches to building them.
 * [Concepts Overview](/nservicebus/concept-overview.md): Descriptions of the concepts, features and vernacular of NServiceBus.
 * [Hosting](/nservicebus/hosting/): Provides information on the various approaches to hosting an instance of NServiceBus.


## Tutorials

The [NServiceBus Quick Start](/tutorials/quickstart/) tutorial gives a quick overview of the benefits of using NServiceBus, and shows how software systems built on asynchronous messaging using NServiceBus are superior to traditional synchronous HTTP-based web services.

The [Introduction to NServiceBus](/tutorials/intro-to-nservicebus/) tutorial gives a guided walk-through of building an NServiceBus system and introduces the basic concepts of messaging. Each lesson includes a downloadable solution with the completed exercise.

The [Scaling Applications with Microservices and NServiceBus 6](https://www.pluralsight.com/courses/microservices-nservicebus6-scaling-applications) Pluralsight course provides guidance on MicroServices and how to build them using NServiceBus.


## Samples

Samples are a great way to learn and explore NServiceBus by looking at runnable code. They are fully functional solutions that can be run from Visual Studio. Learn by exploring code. Each sample includes an explanation of the scenario and the technology or concept that it illustrates.

Go to [Introduction to Sample](/samples/) or [Skip to the list of samples](/samples/#related-samples).

The following samples are recommended for getting started:

 * [Step by Step Sample](/samples/step-by-step/): Illustrates essential NServiceBus concepts by showing how to build a simple system.
 * [Endpoint configuration choices](/samples/endpoint-configuration/): Walks through the most common choices required when creating a first endpoint. It will also show the configuration APIs needed to implement those choices.
 * [On Premise Show Case](/samples/show-case/on-premise/): An implementation of a fictional store that shows many features of NServiceBus working together.


## Downloading

NServiceBus consists of a many components (some optional) which are deployed through [NuGet Packages](https://www.nuget.org). The most important elements, required in most scenarios, are the [transport](/transports/) and [persistence](/persistence/). The transport is an abstraction over low-level messaging infrastructure (e.g. MSMQ or SQL Server), the persistence provides support for some NServiceBus features like [delayed-delivery](/nservicebus/messaging/delayed-delivery.md), [publish-subscribe](/nservicebus/messaging/publish-subscribe/) and [sagas](/nservicebus/sagas/).

The majority of the NuGet packages are listed under the [NServiceBus NuGet User](https://www.nuget.org/profiles/nservicebus) with the main library is deployed via the [NServiceBus NuGet package](https://www.nuget.org/packages/NServiceBus/).

```
PM> Install-Package NServiceBus
```

There are also many of extensions and utilities that build on the Platform in a variety of ways. These are listed under [Extensions](/components/).

The other parts of the Platform ([ServiceControl](/servicecontrol/), [ServiceInsight](/serviceinsight/) and [ServicePulse](/servicepulse/)) are deployed through either the [Platform Installer](/platform/installer/) (mainly for development environment) or via a [Direct Download](https://particular.net/downloads) of an installer (mainly for production and integration environments).


## Getting Help / More Information

 * [Particular Software Support](https://particular.net/support)
 * [Licensing/Sales information](https://particular.net/licensing)
 * [Contact Particular Software](https://particular.net/contactus)
 * [Get help with a Proof-Of-Concept](https://particular.net/proof-of-concept)
 * [Community Discussion Group](https://discuss.particular.net)
 * [GitHub/docs.particular.net](https://github.com/Particular/docs.particular.net) for any problems with the documentation content.
 * Any issues, bugs or feature requests with the Platform or any extension can be raise in the specific projects. See the "Project Hosting" links on the [Extensions](/components/) page.
