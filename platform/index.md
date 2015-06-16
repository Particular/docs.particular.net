---
title: Particular Service Platform Overview
summary: A short overview of Particular Platform
tags: 
- Particular Service Platform
- NServiceBus
- ServiceMatrix
- ServiceControl
- ServiceInsight
- ServicePulse
redirects:
 - nservicebus/preparing-your-machine-to-run-nservicebus
---

# Particular Service Platform

- [Getting started with the Particular Service Platform](getting-started-with-particular-service-platform.md)
- [Releases and Release Notes](release-notes.md)
- [Platform Installer](installer)

Everything started with a simple <cite>wrapper library around System Messaging API</cite>. Few years later, it was shared with wider public in the form of an open-source project. Following the popular .NET convention it was called simply NServiceBus. The project gained popularity and soon it became clear that it would be difficult to keep meeting user demands without a dedicated team of developers. That is how the Particular Software company came to life. 

Messaging is fantastic for building loosely coupled, scalable and reliable systems. However, it also has challenges. The two most common pains are having visibility of what is happening in the system. In the beginning teams using NServiceBus relied on generic monitoring applications or they built their own. After a while it became obvious that they could benefit from tools tailored to specific needs of NServiceBus.

That is how the idea of the Particular Service Platform was born. The goal is to provide a set of tools that make building and maintenance of messaging systems easier. The tools are tailored to common needs of a messaging system and 'just work' out of the box, thus enabling developers to focus on other important challenges, such as understanding their business domain better. At the moment, Particular Service Platform consists of [NServiceBus](/nservicebus), [ServiceMatrix](/servicematrix), [ServiceControl](/servicecontrol), [ServiceInsight](/serviceinsight) and [ServicePulse](/servicepulse).

<img src="architecture_overview.png" title="Particular Service Platform architecture">

# NServiceBus - where it all begins

NServiceBus is the heart of the system. It is a messaging and workflow framework that will help you to create distributed systems that are scalable, reliable and easy to modify. It supports various messaging patterns, offers support for handling long-running business processes in the form of [sagas](/nservicebus/sagas.md) and provides abstraction over multiple [queueing technologies](/nservicebus/transports.md), which guarantees that each message will be delivered once and only once. It automatically solves intermittent problems, by retrying messages and if that does not solve the issue, it sends them to the error queue, where they wait for human intervention. 

Moreover, NServiceBus is very extensible. You can tailor it to your needs, select technologies you are already familiar with or create customized versions of various elements in the system. 

# ServiceControl - gather all the data

ServiceControl is the brain of monitoring in Particular Service Platform. It collects data on every single message flowing through your system (Audit Queue), errors (Error Queue), as well as additional information regarding sagas, endpoints heartbeats and custom checks (Control Queue). The information then is exposed to ServicePulse and ServiceInsight via HTTP API and SignalR notifications.

It is important to understand that the data is still collected even if ServiceControl is down. When it starts working again it will process all the information that was saved in the meantime.

In order to allow ServiceControl for gathering information, you need to configure your solution properly:

* [enable auditing](/nservicebus/operations/auditing.md) to collect data on individual messages;
* configure [error queue](/nservicebus/errors.md) to store information on messages failures;
* [install plugins on your endpoints](/servicecontrol/plugins.md) to monitor their health, sagas and use custom checks.

By default ServiceControl stores information for 30 days, but you can easily [customize it](/servicecontrol/creating-config-file.md).

# ServiceInsight - invaluable developer tool

ServiceInsight is a desktop application with features tailored to developers needs. It allows for advanced debugging, tracking the flow of the individual message in the system, observing sagas and more. 

Thanks to the rich visual content, such as flow diagrams, it is much easier to quickly spot anomalies and incorrect behaviour. At the same time, it gives access to more detailed information, such as message headers and all message metadata. For more information, including sample screens refer to [documentation](/serviceinsight/getting-started-overview.md).


# ServicePulse - production monitoring

ServicePulse is a web application dedicated mainly to administrators. It gives you a clear, near real-time, high-level overview of the system. 

There you will get notified when the endpoint is down or when a message fails. You can also specify your own custom checks and get alerts. The interface allows you to perform the common operations for failure recovery, such as restarting failed messages. You can also [subscribe to publicly exposed events](/servicepulse/custom-notification-and-alerting-using-servicecontrol-events.md), in order to display and handle them in a custom way.


# ServiceMatrix - faster prototyping

ServiceMatrix is a VisualStudio extension, that allows for creating messaging system with NServiceBus using visual tools. With a few simple clicks you can visually create a prototype on canvas. You can also easily extend the generated code to customize your prototype.

ServiceMatrix is perfect for Proof of Concept, prototyping and discussing project design with a team of developers. Having a working prototype with visualization helps to clarify the design better in terms of how the system works. Combined with the run time visualization from ServiceInsight, the team can see how information and messages flow through the various endpoints using message flow and sequence diagrams.

# How do you work with the platform?
Having ServiceControl and ServiceInsight installed locally on your machine would give you a lot of benefit during development, especially when you investigate failures and defects. Additionally, if you develop a custom check in ServicePulse, it would be useful to have ServicePulse installed locally in order to debug it.

After solution is deployed, you should have ServiceControl and ServicePulse in each environment it was deployed to (e.g. one instance per INTEGRATION, another one for TEST and one more for PROD). ServiceInsight is a client install, so you can point it to the specific environment or local instance url.

# More information 

For more information and commonly asked questions, refer to documentation regarding specific tools: 

[![NServiceBus article index](/menu/nservicebus-logo.png)](/nservicebus)

[![ServiceMatrix article index](/menu/servicematrix-logo.png)](/servicematrix)

[![ServiceInsight article index](/menu/serviceinsight-logo.png)](/serviceinsight)

[![ServicePulse article index](/menu/servicepulse-logo.png)](/servicepulse)

[![ServiceControl article index](/menu/servicecontrol-logo.png)](/servicecontrol)

To see demos showing how those tools are used and to understand the principles behind the platform even better, watch the videos:

* [Deep dive into Service Matrix and Service Insight](http://fast.wistia.net/embed/iframe/4348umnahj?videoFoam=true&autoPlay=true), 
* [Production monitoring using ServicePulse](http://fast.wistia.net/embed/iframe/v6s8xcyh31?videoFoam=true&autoPlay=true).