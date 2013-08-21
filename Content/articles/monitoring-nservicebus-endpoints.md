<!--
title: "Monitoring NServiceBus Endpoints"
tags: 
-->

Monitoring in NServiceBus is easier than in regular three-tier systems due to the use of queuing and message-based communication.

When a system is broken down into multiple processes, each with its own queue, you can quickly identify which process is the bottleneck by examining how many messages (on average) are in each queue. The only issue is that without knowing the rate of messages coming into each queue, and the rate at which messages are being processed from each queue, you can't know how long messages are waiting in each queue, which is the primary indicator of a bottleneck.

Unfortunately, despite the many performance counters Microsoft provides for MSMQ (including messages in queues, machine-wide incoming and outgoing messages per second, and the total messages in all queues), there is no built-in performance counter for the time it takes a message to get through each queue.

NServiceBus performance counters
--------------------------------

As a part of the NServiceBus installation, two performance counters are installed under the new "NServiceBus" category.

-   "Critical time" monitors the age of the oldest message in the queue.
    This takes into account the whole chain, from the message being sent
    from the client machine until successfully processed by the server.
    Define an SLA for each of your endpoints and use the CriticalTime
    counter to make sure you adhere to it.
-   "Time to SLA breach" acts as a early warning system to tell you the
    number of seconds left until the SLA for the particular endpoint is
    breached. This gives you a system-wide counter that can be monitored
    without putting the SLA into your monitoring software. Just set that
    alarm to trigger when the counter goes below X, which is the time
    that your operations team needs to be able to take actions to
    prevent the SLA from being breached. To define the endpoint SLA, add
    the [EndpointSLA] attribute on your endpoint configuration.If
    self-hosting, use the Configure.SetEndpointSLA() method on the
    Fluent API instead. All processes running with the NServiceBus
    collect this information and the counters are enabled by default.

Since all performance counters in Windows are exposed via Windows Management Instrumentation (WMI), it is very straightforward to pull this information into your existing monitoring infrastructure.

The following video shows NServiceBus performance counters, and demonstrates their use.

<iframe allowfullscreen frameborder="0" src="http://www.youtube.com/embed/gKLHT7Kj4Rg"></iframe>

Best practices
--------------

If the monitored system is designed according to the NServiceBus best practice of having each process (and by corollary each queue) handle only a single message type, you can then know how long each type of messages is waiting in the system. This enables you to provide the business with information on a use-case by use-case basis. The business can, in turn, specify SLA requirements per use case, which can then be monitored.

Based on this information, each process can be scaled independently using [the distributor](load-balancing-with-the-distributor.md) to make sure it stays within required service levels. This is Business Service Management (BSM) at its finest.

Read about NServiceBus support for
[auditing](auditing-with-nservicebus.md).

