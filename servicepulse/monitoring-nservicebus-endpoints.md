---
title: Monitoring NServiceBus Endpoints
summary: Introduction to monitoring approaches and tools for NServiceBus endpoints
tags:
- ServicePulse
- Monitoring
- Auditing
- Service Level Agreement
- Heartbeat
---
Monitoring in NServiceBus is easier than in regular three-tier systems due to the use of queuing and message-based communication. Given that generally messages are an atomic piece of information that drives the system, it is enough to know where a message comes from, what it has changed as a consequence of the handling process, and where it is going.

This article covers the following topics:

* Monitoring primers
* ServicePulse
* Service Level Agreement
* Performance counters
* Audit and error queues
* Heartbeat and checks
* Best practices

##Monitoring Primers

When a system is broken down into multiple parts, monitoring becomes a key aspect not only from a system maintenance, DevOps, point of view, but also from the system behavior point of view. Losing control of what the system does and why the system behaves in a certain way can rapidly become a difficult problem to manage.

NServiceBus monitoring tools and practices leverage the intrinsic power that a messaging architecture brings to the table to allow an easy and powerful monitoring process.

The messaging-based system allows you to identify which process is the bottleneck by examining how many messages are in each queue. You can quickly understand where a message is stuck and why, and you can identify reasons that cause a message to be delivered to a specific endpoint by tracking its route from beginning to end.

##ServicePulse

ServiceControl is the heart of the NServiceBus monitoring infrastructure. Its role is to collect and store information to be  processed later by tools such as [ServicePulse](/servicepulse/#introduction).

ServicePulse is the front end of ServiceControl. Through ServicePulse, administrators can monitor the overall health of the entire system and be notified of failed messages that flow into error queues.

###Heartbeats and Checks

To allow endpoints to communicate their status to the ServiceControl and ServicePulse monitoring tools, the concepts of [heartbeat and checks](/servicepulse/how-to-configure-endpoints-for-monitoring.md) are introduced:

* **Hearbeat**: each endpoint can send a [heartbeat message](/servicepulse/intro-endpoints-heartbeats.md) to the monitoring infrastructure, signaling that it is alive. Simply deploy a plugin to the endpoint and restart it.
* **Checks**: you can develop and deploy [custom checks](/servicepulse/intro-endpoints-custom-checks.md) to the endpoint, as plugins, to enrich the information that the endpoint sends to the monitoring infrastructure.

##Service Level Agreement (SLA)

An SLA is an agreement between involved parties. Regarding endpoints and messages, an SLA is a way to express that a given endpoint must consume——*handle* in NServiceBus terminology——messages within a given amount of time, otherwise the SLA itself is not respected.

##Performance Counters

Performance counters are one of the foundations of NServiceBus monitoring. Operating system level counters allow us to identify bottlenecks in our endpoints by examining how many messages (on average) are in each queue.

However, without knowing the rate of messages coming into each queue and the rate at which messages are being processed from each queue, you cannot know how long messages are waiting in each queue, which is the primary indicator of a bottleneck.

Unfortunately, despite the many performance counters Microsoft provides for MSMQ (including messages in queues, machine-wide incoming and outgoing messages per second, and the total messages in all queues), there is no built-in performance counter for the time it takes a message to get through each queue.

###NServiceBus Performance Counters

As a part of the NServiceBus installation, two additional performance counters are installed in the new "NServiceBus" category.

* **Critical time** monitors the age of the oldest message in the queue. This takes into account the whole chain, from the message being sent from the client machine until it is successfully processed by the server. Define a SLA for each of your endpoints and use the `CriticalTime` counter to make sure to adhere to it.
* **Time to SLA breach** acts as an early warning system to tell you the number of seconds left until the SLA until the particular endpoint is breached. This system-wide counter can be monitored without putting the SLA into your monitoring software. Just set the alarm to trigger when the counter goes below X, which is the time that your operations team needs to be able to take action to prevent the SLA from being breached. To define the endpoint SLA, add the `[EndpointSLA]` attribute to your endpoint configuration. If self-hosting, use the `Configure.SetEndpointSLA()` method on the Fluent API instead. All processes running with NServiceBus collect this information and the counters are enabled by default. Since all performance counters in Windows are exposed via Windows Management Instrumentation (WMI), it is straightforward to pull this information into your existing monitoring infrastructure.

Details on how to install and manage NServiceBus performance counters are in the [managing NServiceBus with PowerShell](/nservicebus/managing-nservicebus-using-powershell.md) article.

##Audit and Error Queues

###Audit Queues

If the role of performance counters is to monitor the time required by the system to handle messages, the role of the auditing infrastructure is to inspect the content of messages that flows into the system.

Each endpoint can be [configured](/nservicebus/auditing-with-nservicebus.md)——machine-wide or per endpoint——to forward each received message to a dedicated audit queue where a monitoring process such as ServiceControl can handle all the messages, tracking them in a database to allow further processing and analysis.

###Error Queues

When a system is based on a messaging infrastructure it automatically benefits from the durable and persistent nature of the infrastructure, leading to a more robust system where data cannot be lost. At runtime, NServiceBus handles any error that the user code throws while handling the incoming message, and automatically retries the failed message, following a well known and condigurable policy.

It is obvious that a failing message cannot be retried forever without leading to SLA violations or performance penalties to the entire system. For this reason, after a configurable number of retries, NServiceBus stops retrying the message and moves it to the configured error queue, which, as an audit queue, can be defined machine-wide or per endpoint.

Error queues can then be monitored——for example, using ServiceControl as for audit queues——by administrators who are notified when something goes wrong and can react accordingly.

##Best Practices

If the monitored system is designed according to the NServiceBus best practice of having each process (and by corollary each queue) handle only a single message type, you can then know how long each type of messages is waiting in the system. This enables you to provide the business with information on a use-case by use-case basis. The business can, in turn, specify SLA requirements per use case, which can then be monitored.

Based on this information, each process can be scaled independently using the distributor to make sure it stays within required service levels. This is Business Service Management at its finest.
