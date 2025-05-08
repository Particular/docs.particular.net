---
title: The Particular Service Platform
reviewed: 2024-05-23
suppressRelated: true
isLearningPath: true
redirects:
 - nservicebus/preparing-your-machine-to-run-nservicebus
---

Messaging is a great way to build loosely coupled, scalable, and reliable systems. But it has its challenges. The most common difficulty is seeing what's happening in a system. This is one of the problems solved by the Particular Service Platform.

The Platform's goal is to provide a set of tools that make it easier to build and maintain messaging systems. The tools are tailored to the common needs of messaging systems and 'just work', out of the box. They enable developers to focus on more important challenges, such as gaining a better understanding of their business domains.

The Particular Service Platform consists of [NServiceBus](/nservicebus), [ServiceControl](/servicecontrol), [ServicePulse](/servicepulse), and [ServiceInsight](/serviceinsight).

![Particular Service Platform architecture](architecture-overview.png)

The details of each component are discussed below. A Particular Service Platform-based system consists of several NServiceBus [endpoints](/nservicebus/endpoints/). Endpoints are logical entities that perform business operations. They communicate with each other using messages (via queues) and forward messages to ServiceControl for auditing. ServiceControl stores this audit trail and provides integration points for ServicePulse and ServiceInsight. ServicePulse provides monitoring and recoverability for production systems. ServiceInsight and ServicePulse provide debugging information and visualization of how the system works.

## [NServiceBus](/nservicebus) - where it all begins

include: nservicebus

## [ServiceControl](/servicecontrol) - data collection

include: servicecontrol

## [ServicePulse](/servicepulse) - debug, retry and monitor

include: servicepulse

## [ServiceInsight](/serviceinsight) - visualization

include: serviceinsight

## Working with the platform

ServiceControl and ServicePulse are server applications. They should be deployed in each environment, for example: test, QA, and production. ServiceInsight is a client application. It should be deployed on a workstation and connected to ServiceControl in the appropriate environment.

When investigating problems or developing [custom checks](/monitoring/custom-checks/), it can be useful to have the Platform installed on a development machine.
