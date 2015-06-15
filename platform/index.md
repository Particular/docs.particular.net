---
title: Particular Service Platform Overview
summary: A short overview of Particular Platform
tags: 
- Particular Service Platform, NServiceBus, ServiceMatrix, ServiceControl, ServiceInsight, ServicePulse
redirects:
 - nservicebus/preparing-your-machine-to-run-nservicebus
---

# Particular Service Platform

- [Getting started with the Particular Service Platform](getting-started-with-particular-service-platform.md)
- [Releases and Release Notes](release-notes.md)
- [Platform Installer](installer)

Everything started with a simple <cite>wrapper library around System Messaging API [1]</cite>. Few years later, it was shared with wider public in the form of an open-source project. Following the popular .NET convention it was called simply NServiceBus. The project gained popularity and soon it became clear that it would be difficult to keep meeting user demands without a dedicated team of developers. That is how the Particular Software company came to life. 

Messaging is fantastic for building loosely coupled, scalable and reliable systems. However, it also has challenges. Two most common pains are having visibility of what is happening in the system and monitoring. In the beginning, teams using NServiceBus relied on generic monitoring applications or they built their own. After a while it became obvious that they could benefit from tools tailored to specific needs of NServiceBus. The other challenge is to understand the principles of building distributed systems and applying them in practice. This might be especially difficult for less experienced teams or developers that worked in a different way for many years. 

That is how the idea of the Particular Service Platform was born, its goal is to provide a set of tools that 'just work' and make building and maintaining messaging systems easier, enabling the developers to focus on more important problems, such as understanding business domain rather than infrastructure. At the moment, Particular Service Platform consists of [NServiceBus](/nservicebus), [ServiceMatrix](/servicematrix), [ServiceControl](/servicecontrol), [ServiceInsight](/serviceinsight) and [ServicePulse](/servicepulse).

<img src="architecture_overview.png" title="Particular Service Platform architecture">

# NServiceBus - where it all begins

NServiceBus is the heart of the system. It is a messaging and workflow framework that will help you to create distributed systems that are scalable, reliable and easy to modify. It supports various [messaging patterns](nservicebus/messaging/messages-events-commands.md), such as commands, events (publish-subscribe), offers support for handling long-running business processes in the form of [sagas](nservicebus/sagas/) and provides abstraction over multiple [queueing technologies](nservicebus/transports/), which guarantees that each message will be delivered once and only once. It automatically solves intermittent problems, by retrying messages and if that does not solve the issue, it sends them to the error queue, where they wait for human intervention.

NServiceBus is very extensible, you can tailor it to your needs, select technologies you are already familiar with or create customized versions of various elements in the system. For more details refer to the [documentation](nservicebus/).

# ServiceControl - gather all the data

ServiceControl is the brain of monitoring in Particular Service Platform. It collects data on every single message flowing through your system (Audit Queue), errors (Error Queue), as well as additional information regarding sagas, endpoints heartbeats and custom checks (Control Queue). The information then is exposed to ServicePulse and ServiceInsight via HTTP API and SignalR notifications.

It is important to understand, that the data is still collected even if ServiceControl is down. When it starts working again, it will process all the information that was saved in the meantime.

In order to allow ServiceControl for gathering information, you need to configure your solution properly:

* [enable auditing](nservicebus/operations/auditing.md) to collect data on individual messages;
* [install plugins on your endpoints](servicecontrol/plugins.md) to monitor their health, sagas and use custom checks.

By default ServiceControl stores information for 30 days and initiates purging process every minute, but you can easily customize those values using [configuration](servicecontrol/creating-config-file.md) or [command-line arguments](servicecontrol/how-purge-expired-data.md).

The standard approach is to have one ServiceControl installation on a single host. ServicePulse and ServiceInsight can be installed on other machines. You need to configure ServicePulse and ServiceInsight to connect with ServiceControl. You can also make ServiceControl highly available, to avoid having a single point of failure in your monitoring system.

# ServiceInsight - invaluable developer tool

ServiceInsight is a WPF application with features tailored to developers needs. It allows for advanced debugging, tracking the flow of the individual message in the system, observing sagas and more. 

Thanks to the rich visual content, such as flow diagrams, it is much easier to quickly spot anomalies and incorrect behaviour. At the same time, it gives access to more detailed information, such as message headers and all message metadata. For more information, including sample screens refer to [documentation](serviceinsight/getting-started-overview.md).


# ServicePulse - production monitoring

ServicePulse is a web application dedicated mainly to administrators. It gives you a clear, near real-time, high-level overview of the system. 

There you will get notified when the endpoint is down or when a message fails. You can also specify your own custom checks and get alerts. The interface allows you to perform the common operations for failure recovery, such as restarting failed messages. You can also [subscribe to publicly exposed events](servicepulse/event-types.md), in order to display and handle them in a custom way.


# ServiceMatrix - boost your productivity

ServiceMatrix is a VisualStudio extension, that allows for creating messaging system with NServiceBus using visual tools. With a few simple clicks you can generate a fully functional solution and even deploy it. You can also easily extend the generated code.

ServiceMatrix is perfect for Proof of Concept, prototyping and discussing project design while coding kind of work. It also is invaluable when the team is new to messaging concepts, as it is much easier to understand how system works and how information flows through it, having a visual representation of the system that corresponds to the actual code.

# More information 

For more information and commonly asked questions, refer to documentation regarding specific tools: 

[![NServiceBus article index](/menu/nservicebus-logo.png)](/nservicebus)

[![ServiceMatrix article index](/menu/servicematrix-logo.png)](/servicematrix)

[![ServiceInsight article index](/menu/serviceinsight-logo.png)](/serviceinsight)

[![ServicePulse article index](/menu/servicepulse-logo.png)](/servicepulse)

[![ServiceControl article index](/menu/servicecontrol-logo.png)](/servicecontrol)

To see demos showing how those tools are used and to understand the principles behind the platform even better, watch the videos:

* [Particular Service Platform Overview](http://player.vimeo.com/video/99322069?autoplay=1), 
* [Deep dive into Service Matrix and Service Insight](http://fast.wistia.net/embed/iframe/4348umnahj?videoFoam=true&autoPlay=true), 
* [Production monitoring using ServicePulse](http://fast.wistia.net/embed/iframe/v6s8xcyh31?videoFoam=true&autoPlay=true).

[1]: David Boike, "Learning NServiceBus - Second Edition", Introduction