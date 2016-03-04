---
title: Particular Service Platform Overview
summary: A short overview of Particular Platform
tags: []
redirects:
 - nservicebus/preparing-your-machine-to-run-nservicebus
---

Messaging is a good way to build loosely coupled, scalable and reliable systems. However, it also has its challenges. The most common difficulty is having visibility of what's happening in the system. This is one of the problems that the Particular Service Platform was built to solve.

The goal of the Platform is to provide a set of tools that make the building and maintenance of messaging systems easier. The tools are tailored to common needs of a messaging system and 'just work' out of the box, enabling developers to focus on other important challenges such as understanding their business domain better. Currently, the Particular Service Platform consists of [NServiceBus](/nservicebus), [ServiceControl](/servicecontrol), [ServiceInsight](/serviceinsight) and [ServicePulse](/servicepulse).

![Particular Service Platform architecture](architecture-overview.png)


## [NServiceBus](/nservicebus) - where it all begins

NServiceBus, the heart of the system, is a messaging and workflow framework that helps you create distributed systems that are scalable, reliable and easy to modify. It supports various messaging patterns, handles long-running business processes in the form of [sagas](/nservicebus/sagas) and provides abstraction over multiple [queueing technologies](/nservicebus/transports/). While most queueing technologies try to make guarantees regarding 'at least once' or even 'exactly once' delivery, they often fall short of this promise. NServiceBus contains mechanisms to automatically solve intermittent delivery problems by retrying messages and falling back to an error queue where they can be exposed by the rest of the Platform for human intervention (ServiceControl, ServicePulse).

Moreover, NServiceBus is thoroughly extensible. It can be tailor it to many requirements, compatible with many technologies and many elements of the system can be replaced with custom implementations.


## [ServiceControl](/servicecontrol) - the foundation

ServiceControl is the monitoring brain in the Particular Service Platform. It collects data on every single message flowing through the system (Audit Queue), errors (Error Queue), as well as additional information regarding sagas, endpoints heartbeats and custom checks (Control Queue). The information is then exposed to [ServicePulse](/servicepulse) and [ServiceInsight](/serviceinsight) via an HTTP API and SignalR notifications.

It is important to understand that the data is still collected even if ServiceControl is down. When it starts working again, it will process all the information that was saved in the meantime.

To enable [ServiceControl](/servicecontrol) to gather this information, you need to configure the solution appropriately:

 * [enable auditing](/nservicebus/operations/auditing.md) to collect data on individual messages;
 * configure the [error queue](/nservicebus/errors) to store information on messages failures;
 * [install plugins on the endpoints](/servicecontrol/plugins/) to monitor their health and sagas and use custom checks.

By default ServiceControl stores information for 30 days, but you can easily [customize this](/servicecontrol/creating-config-file.md).


## [ServiceInsight](/serviceinsight) - message flow visualization

ServiceInsight is a desktop application with features tailored to developers needs. It allows for advanced debugging, tracking the flow of an individual message in the system, observing sagas and more.

It is much easier to quickly spot anomalies and incorrect behavior in the system when you are presented with information in visual form, such as message flow diagrams or sequence diagrams. At the same time, you can access more detailed information such as message headers and all message metadata.


## [ServicePulse](/servicepulse) - production monitoring

ServicePulse is a web application aimed mainly at administrators. It gives a clear, near real-time, high-level overview of how a system is functioning.

Notifications will occur when the endpoint is down or when a message fails. [Custom checks](/servicecontrol/plugins/custom-checks.md) can be built to get alerts for specify your own scenarios. The interface allows you to perform the common operations for failure recovery, such as retrying failed messages. You can also [subscribe to publicly exposed events](/servicecontrol/contracts.md), in order to display and handle them in a custom way.


## Working with the platform

Having ServiceControl and ServiceInsight installed locally on a machine gives significant benefits during development, especially when investigating failures and defects. Additionally, if developing a [custom check](/servicecontrol/plugins/custom-checks.md) it is useful to have the full platform installed on a development machine.

After a solution is deployed, ServiceControl and ServicePulse should exist in each environment it was deployed to (e.g. one instance per integration, another one for test and one more for production). ServiceInsight is a client install, so it can be installed on a local machine only and point it to the specific environment or local instance URL.
