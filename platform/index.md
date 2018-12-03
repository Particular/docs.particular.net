---
title: Particular Service Platform Overview
reviewed: 2018-11-27
redirects:
 - nservicebus/preparing-your-machine-to-run-nservicebus
---

Messaging is a good way to build loosely coupled, scalable, and reliable systems. But it also has its challenges. The most common difficulty is seeing what's happening in the system. This is one of the problems that the Particular Service Platform was built to solve.

The goal of the Platform is to provide a set of tools that make the building and maintenance of messaging systems easier. The tools are tailored to common needs of a messaging system and 'just work' out of the box, enabling developers to focus on other important challenges such as understanding their business domain better. The Particular Service Platform consists of [NServiceBus](/nservicebus), [ServiceControl](/servicecontrol), [ServiceInsight](/serviceinsight), and [ServicePulse](/servicepulse).

![Particular Service Platform architecture](architecture-overview.png)

The details of each component are discussed below but in general, a Particular Service Platform-based system consists of several NServiceBus [endpoints](/nservicebus/endpoints/). These are logical entities that communicate with each other using messages (via queues) in order to perform business operations. This message activity is audited by ServiceControl, which provides integration points for ServicePulse to monitor production systems, and for ServiceInsight to provide debugging and visualization capabilities into how the system works.


## [NServiceBus](/nservicebus) - where it all begins

include: nservicebus


## [ServiceControl](/servicecontrol) - the foundation

include: servicecontrol


## [ServiceInsight](/serviceinsight) - message flow visualization

include: serviceinsight


## [ServicePulse](/servicepulse) - production monitoring

include: servicepulse


## Working with the platform

Having ServiceControl and ServiceInsight installed locally on a machine gives significant benefits during development, especially when investigating failures and defects. Additionally, if developing a [custom check](/monitoring/custom-checks/) it is useful to have the full platform installed on a development machine.

After a solution is deployed, ServiceControl and ServicePulse should exist in each environment it was deployed to (e.g. one instance per integration, another one for test and one more for production). ServiceInsight is a client install, so it can be installed on a local machine only and point it to the specific environment or local instance URL.
