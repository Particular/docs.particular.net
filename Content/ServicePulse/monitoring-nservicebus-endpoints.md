---
title: Monitoring NServiceBus Endpoints
summary: Introduction on NServiceBus endpoints monitoring approaches and tools
tags:
- ServicePulse
- Monitoring
- Audting
- Service Level Agreement
- Heartbeat
---
Monitoring in NServiceBus is easier than in regular three-tier systems due to the use of queuing and message-based communication. Given the fact that generally messages are an atomic piece of information that drives the system we can say that it is enought to know where a message comes from, what it has changed, as a consequance of the handling process, and finally where it is going.

This article will cover the following topics:

* Monitoring primers;
* Service Level Agreement;
* Performance Counters;
* Audit and Error queues;
* ServicePulse;
* Heartbeat and Checks;

##Monitoring Primers

When a system is broken down into multiple processes monitoring becomes a key aspect not only from a system maintenance, DevOps, point of view, but also from the system behavior point of view. Losing control of what the system does and why the system behaves in a certain way can rapidly become a problem hard to manage.

NServiceBus monitoring tools and practices levarage the intrinsic power that a messaging architecture brings to the table to allow an easy and powerfull monitoring process.

When a system is a messaging based system you can quickly identify which process is the bottleneck by examining how many messages are in each queue; you can quickly understand where a message is stuck and why and you can quickly indentify reasons that caused a message to be delivered to a specific endpoint by tracking its route from the beginning to the end.

##Service Level Agreement

A SLA, as the name implies, is an agreement between the involved parts. When speaking about endpoints and messages a SLA is a mean to express that a given endpoint must consume, handle in NServiceBus terminology, messages within a given amount of time, otherwise the SLA itself is not respected.

##Performance Counters

Performance Counters are one of the NServiceBus monitoring foundation, Operating System level counters allows us to identify bottlenecks in our endpoints by examining how many messages (on average) are in each queue.

The only issue is that without knowing the rate of messages coming into each queue, and the rate at which messages are being processed from each queue, you can't know how long messages are waiting in each queue, which is the primary indicator of a bottleneck.

Unfortunately, despite the many performance counters Microsoft provides for MSMQ (including messages in queues, machine-wide incoming and outgoing messages per second, and the total messages in all queues), there is no built-in performance counter for the time it takes a message to get through each queue.

###NServiceBus performance counters

As a part of the NServiceBus installation, two additional performance counters are installed under the new "NServiceBus" category.

* **Critical time** monitors the age of the oldest message in the queue. This takes into account the whole chain, from the message being sent from the client machine until successfully processed by the server. Define a SLA for each of your endpoints and use the `CriticalTime` counter to make sure you adhere to it.
* **Time to SLA breach** acts as a early warning system to tell you the number of seconds left until the SLA for the particular endpoint is breached. This gives you a system-wide counter that can be monitored without putting the SLA into your monitoring software. Just set that alarm to trigger when the counter goes below X, which is the time that your operations team needs to be able to take actions to prevent the SLA from being breached. To define the endpoint SLA, add the `[EndpointSLA]` attribute on your endpoint configuration. If self-hosting, use the `Configure.SetEndpointSLA()` method on the Fluent API instead. All processes running with NServiceBus collect this information and the counters are enabled by default. Since all performance counters in Windows are exposed via Windows Management Instrumentation (WMI), it is very straightforward to pull this information into your existing monitoring infrastructure.

##Audit and Error queues

###Audit queues

If performance counters role is to monitor the time required by the system to handle messages the role of the auditing infrastructure is to inspect the content of messages that flows into the system.

Each endpoint can be [configured](/NServiceBus/auditing-with-nservicebus), machine-wide or per endpoint, to foward each received message to a dedicated audit queue where a monitoring process, such as ServiceControl, can handle all the messages tracking them in a database to allow further processing and analysis.

###Error queues

When a system is based on a messaging infrastructure it automatically benefits of the durability and the persistent nature of the infrastructure leading to more robust system where data cannot be lost. At runtime what NServiceBus does is to handle any error that the user code can throw while handling the incoming message and will automatically retry the failed message following a well known and condigurable policy.

It is obvious that a failing message cannot be retried forever without leading to SLA violations or performance penalties to the entire system, for this reason after a configurable amount of retries NServiceBus stops retrying the message and moves it the the configured error queue, that, as audit queues, can be defined machine-wide or per endpoint.

Error queues can then be monitored, for example using ServiceControl as for audit queues, by administrators to be notified when something goes wrong and be able to react accordingly.

##ServicePulse

ServiceControl is the heart of the NServiceBus monitoring infrastructure, its role is to collect and store information to be later processed by tools such as ServicePulse.

ServicePulse is the primary frontline front-end to ServiceControl, throught ServicePulse administrators can monitor the overall health of the entire system and can be notified of failed messages that flows into error queues.

###Heartbeat and Checks

In order to allow endpoints to communicate their status to the ServiceControl and ServicePulse monitoring tools the concepts of [Heartbeat and Checks](/ServicePulse/how-to-configure-endpoints-for-monitoring) has been introduced:

* **Hearbeat**: each endpoint can send a heartbeat message to the monitoring infrastructure signaling that it is alive; This can be easily achived by deploying a plugin to the endpoint and restarting it;
* **Checks**: Custom checks can be developed and deployed to the endpoint, always as plugins, to enrich the information that the endpoint sends to the monitoring infrastructure;

##Best practices

If the monitored system is designed according to the NServiceBus best practice of having each process (and by corollary each queue) handle only a single message type, you can then know how long each type of messages is waiting in the system. This enables you to provide the business with information on a use-case by use-case basis. The business can, in turn, specify SLA requirements per use case, which can then be monitored.

Based on this information, each process can be scaled independently using the distributor to make sure it stays within required service levels. This is Business Service Management (BSM) at its finest.
