---
title: Particular Service Platform Overview
summary: A short overview of Particular Platform
tags: []
redirects:
 - nservicebus/preparing-your-machine-to-run-nservicebus
---

Messaging is fantastic for building loosely coupled, scalable and reliable systems. However, it also has challenges. The most common difficulty is having visibility of what is happening in the system. This is one of the problems that Particular Service Platform was built to solve.

The goal of the Platform is to provide a set of tools that make building and maintenance of messaging systems easier. The tools are tailored to common needs of a messaging system and 'just work' out of the box, thus enabling developers to focus on other important challenges, such as understanding their business domain better. At the moment, Particular Service Platform consists of [NServiceBus](/nservicebus), [ServiceMatrix](/servicematrix), [ServiceControl](/servicecontrol), [ServiceInsight](/serviceinsight) and [ServicePulse](/servicepulse).

![Particular Service Platform architecture](architecture-overview.png)


## [NServiceBus](/nservicebus) - where it all begins

NServiceBus is the heart of the system. It is a messaging and workflow framework that will help you to create distributed systems that are scalable, reliable and easy to modify. It supports various messaging patterns, offers support for handling long-running business processes in the form of [sagas](/nservicebus/sagas) and provides abstraction over multiple [queueing technologies](/nservicebus/transports/), which guarantees that each message will be delivered once and only once. It automatically solves intermittent problems, by retrying messages and if that does not solve the issue, it sends them to the error queue, where they wait for human intervention. 

Moreover, NServiceBus is very extensible. You can tailor it to your needs, select technologies you are already familiar with or create customized versions of various elements in the system. 


## [ServiceControl](/servicecontrol) - gather all the data

ServiceControl is the brain of monitoring in Particular Service Platform. It collects data on every single message flowing through your system (Audit Queue), errors (Error Queue), as well as additional information regarding sagas, endpoints heartbeats and custom checks (Control Queue). The information then is exposed to ServicePulse and ServiceInsight via HTTP API and SignalR notifications.

It is important to understand that the data is still collected even if ServiceControl is down. When it starts working again it will process all the information that was saved in the meantime.

In order to allow ServiceControl for gathering information, you need to configure your solution properly:

* [enable auditing](/nservicebus/operations/auditing.md) to collect data on individual messages;
* configure [error queue](/nservicebus/errors) to store information on messages failures;
* [install plugins on your endpoints](/servicecontrol/plugins.md) to monitor their health, sagas and use custom checks.

By default ServiceControl stores information for 30 days, but you can easily [customize it](/servicecontrol/creating-config-file.md).

## [ServiceInsight](/serviceinsight) - invaluable developer tool

ServiceInsight is a desktop application with features tailored to developers needs. It allows for advanced debugging, tracking the flow of the individual message in the system, observing sagas and more. 

It is much easier to quickly spot anomalies and incorrect behavior in your system, when you are presented with information in visual form, such as message flow diagrams or sequence diagrams. At the same time, you can access more detailed information, such as message headers and all message metadata. For more information, including sample screens refer to [documentation](/serviceinsight/getting-started-overview.md).


## [ServicePulse](/servicepulse) - production monitoring

ServicePulse is a web application dedicated mainly to administrators. It gives you a clear, near real-time, high-level overview of the system. 

There you will get notified when the endpoint is down or when a message fails. You can also specify your own custom checks and get alerts. The interface allows you to perform the common operations for failure recovery, such as restarting failed messages. You can also [subscribe to publicly exposed events](/servicepulse/custom-notification-and-alerting-using-servicecontrol-events.md), in order to display and handle them in a custom way.


## [ServiceMatrix](/servicematrix) - faster prototyping

ServiceMatrix is a Visual Studio extension available with VS2012 and VS2013. As an Architect or a Systems Design Engineer, you can quickly design your system on the design canvas and also generate the prototype based off your design. You can customize the generated code to suit your needs for the prototype.

ServiceMatrix is perfect for Proof of Concept, prototyping and discussing project design with a team of developers. Having a working prototype with visualization helps to clarify the design better in terms of how the system works. Combined with the run time visualization from ServiceInsight, the team can see how information and messages flow through the various endpoints using message flow and sequence diagrams.


## How do you work with the platform?

Having ServiceControl and ServiceInsight installed locally on your machine gives you significant benefits during development, especially when you investigate failures and defects. Additionally, if you develop a [custom check](/servicecontrol/plugins.md#customchecks-plugin) it is useful to the full platform installed in your development machine.

After solution is deployed, you should have ServiceControl and ServicePulse in each environment it was deployed to (e.g. one instance per INTEGRATION, another one for TEST and one more for PROD). ServiceInsight is a client install, so you can have it on your local machine only and point it to the specific environment or local instance url.
