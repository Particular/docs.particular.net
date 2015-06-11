---
title: Particular Service Platform Overview
summary: A short overview of Particular Platform.
tags: 
- Particular Service Platform, NServiceBus, ServiceControl, ServiceInsight, ServicePulse
---

# Introduction

Everything started in the late 90s/early 2000s with a 'lightweight wrapper around MSMQ'. Few years later, it was shared with wider public as open-source. Following the popular .NET convention it was called simply NServiceBus. The project gained popularity and soon it became clear that it needs more work than the open source projects usually get. That is how the Particular Software company came to life. 

Messaging is fantastic for building loosely coupled, scalable and reliable systems. However, it also has some challenges. Two most common pains are visibility and monitoring. The teams using NServiceBus had to rely on generic monitoring tools or they built their own. After a while it became obvious that they could benefit from tools tailored to NServiceBus. That is how the idea of the Particular Service Platform was born.

At the moment, Particular Service Platform consists of [NServiceBus](http://docs.particular.net/nservicebus/), [ServiceMatrix](http://docs.particular.net/servicematrix/), [ServiceControl](http://docs.particular.net/servicecontrol/), [ServiceInsight](http://docs.particular.net/serviceinsight/) and [ServicePulse](http://docs.particular.net/servicepulse/).

<img src="architecture_overview.png" title="Particular Service Platform architecture">

# NServiceBus - where it all begins

NServiceBus is the heart of the system. It's a messaging and workflow 


# ServiceControl - gather all the data

ServiceControl is the brain of monitoring in Particular Service Platform. It collects data on every single message flowing through your system (Audit Queue), errors (Error Queue) and additional information regarding endpoints health, such as heartbeats, sagas, custom checks (Control Queue). The information then is exposed to ServicePulse and ServiceInsight via HTTP API and SignalR notifications.

It is important to understand, that the data is sent to the queues even if ServiceControl is down. When it starts working again, it will process all the information that came in the meantime.

In order to allow ServiceControl for gathering information, you need to configure your solution properly:

* [enable auditing](http://docs.particular.net/nservicebus/operations/auditing) to collect data on individual messages;
* [install plugins on your endpoints](http://docs.particular.net/servicecontrol/plugins) to monitor their health, sagas and use custom checks.

By default ServiceControl stores information for 30 days and initiates purging process every minute, but you can easily customize those values using [configuration](http://docs.particular.net/servicecontrol/creating-config-file) or [command-line arguments]((http://docs.particular.net/servicecontrol/how-purge-expired-data)).

# ServiceInsight - invaluable developer tool


ServiceInsight is a WPF application with features tailored to common developers needs.
ServiceInsight is dedicated to developers, so it has features that allow you to see in more detail what's going on with specific messages, such as debugging, observing flow of messages in the system, saga view, etc.

# ServicePulse - production monitoring

ServicePulse is a web application dedicated mainly to administrators. It gives you a clear, near real-time, high-level overview of the system. 

There you will get notified when the endpoint is down or when a message fails. You can also specify your own custom checks and get alerts. The interface allows you to perform the common operations for failure recovery, such as restarting failed messages. You can also [subscribe to publicly exposed events](http://docs.particular.net/servicepulse/event-types), in order to display and handle them in a custom way.

The standard approach is to have one ServiceControl installation on a single host. ServicePulse and ServiceInsight can be installed on other machines. You need to configure ServicePulse and ServiceInsight to connect with ServiceControl. You can also make ServiceControl highly available, to avoid having a single point of failure in your monitoring system.

## ServiceMatrix - boost your productivity

ServiceMatrix is a VisualStudio extension, that allows for creating messaging system with NServiceBus using visual tools. With a few simple clicks you can generate fully functional solution and even deploy it. You can also easily extend the generated code.

ServiceMatrix is perfect for Proof of Concept, prototyping and discussing project design while coding kind of work. It also is invaluable when the team is new to messaging concepts, as it is much easier to understand how system works and how information flows through it, having a visual representation of the system that corresponds to the actual code.



# More information 

For more information and commonly asked questions, refer to documentation regarding specific tools: [NServiceBus](http://docs.particular.net/nservicebus/concept-overview), [ServicePulse](http://docs.particular.net/servicepulse/), [ServiceInsight](http://docs.particular.net/serviceinsight/), [ServiceControl](http://docs.particular.net/servicecontrol/) and [ServiceMatrix](http://docs.particular.net/servicematrix/).

To see demos showing how those tools are used and to understand the principles behind the platform even better, watch the videos:

* [Particular Service Platform Overview](http://player.vimeo.com/video/99322069?autoplay=1), 
* [Deep dive into Service Matrix and Service Insight](http://fast.wistia.net/embed/iframe/4348umnahj?videoFoam=true&autoPlay=true), 
* [Production monitoring using ServicePulse](http://fast.wistia.net/embed/iframe/v6s8xcyh31?videoFoam=true&autoPlay=true).

