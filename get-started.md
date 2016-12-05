---
title: Getting Started
summary: Quick list of instructions and hints to get started with NServiceBus and the Particular Service Platform
reviewed: 2016-12-05
---

There are a variety of starting points for NServiceBus and the platform. The path taken will depend on an individuals specific experience and preferences for learning.


## High Level Content

There are many articles that explain concepts and technologies used in the platform. Some good articles when getting started are:

 * [Particular Service Platform Overview](/platform/): Provides an overview of the various parts of Platform; [NServiceBus](/nservicebus/), [ServiceControl](/servicecontrol/), [ServiceInsight](/serviceinsight/) and [ServicePulse](/servicepulse/).
 * [Concepts Overview](/nservicebus/concept-overview.md): A high level overview of the concepts, features and vernacular of NServiceBus.


## Tutorials

There is a [Messaging Basics Tutorial](/tutorials/nservicebus-101/) that gives a guided walk though of building a NServiceBus system and introduces the basic concepts of messaging. The tutorial also has a downloadable and runnable solution to illustrate the finished product.


## Go straight to runnable code

Samples take a "download first" approach and then provided an expatiation of how the given scenario, technology or concept.

Go to [Introduction to Sample](/samples/) or [Skip to the list of samples](/samples/#related-samples).

There are some specific samples that will help get started with NServiceBus:

 * [Step by Step Sample](/samples/step-by-step/): Illustrates essential NServiceBus concepts by showing how to build a simple system
 * [Endpoint configuration choices](/samples/endpoint-configuration/): Walks through the most common choices required when creating a first endpoint. It will also show the configuration APIs needed to implement those choices.


## Getting the bits

The core parts of NServiceBus are deployed through [NuGet Packages](https://www.nuget.org). The majority of the NuGet packages are listed under the [NServiceBus NuGet User](https://www.nuget.org/profiles/nservicebus) with the main library is deployed via the [NServiceBus NuGet package](https://www.nuget.org/packages/NServiceBus/).

```no-highlight
PM> Install-Package NServiceBus
```

The other parts of the Platform ([ServiceControl](/servicecontrol/), [ServiceInsight](/serviceinsight/) and [ServicePulse](/servicepulse/)) are deployed through the [Platform Installer](/platform/installer/).

There are also many of extensions and utilities that build on the platform in a variety of ways. These are listed under [Extensions](/components/).


## Getting Help

 * [Particular Software Support](https://particular.net/support)
 * [Licensing/Sales information](https://particular.net/licensing)
 * [Contact Particular Software](https://particular.net/contactus)
 * [Community Google Discussion Group](https://groups.google.com/d/forum/particularsoftware)
 * [GitHub/docs.particular.net](https://github.com/Particular/docs.particular.net) for any problems with the documentation content.
 * Any issues, bugs or feature requests with any extension to the platform can be raise in the specific project for that extension. See the "Project Hosting" links on the [Extensions](/components/) page.