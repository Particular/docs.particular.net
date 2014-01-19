---
title: Load Balancing with the Distributor
summary: The distributor maintains all the characteristics of NServiceBus but is designed never to overwhelm any of the worker nodes.
originalUrl: http://www.particular.net/articles/load-balancing-with-the-distributor
tags:
- Scaling out
- Distributor
createdDate: 2013-05-20T12:04:24Z
modifiedDate: 2013-08-09T20:14:37Z
authors: []
reviewers: []
contributors: []
---

The NServiceBus Distributor is similar in behavior to standard load balancers. It is the key to transparently scaling out message processing over many machines.

As a standard NServiceBus process, the distributor maintains all the fault-tolerant and performance characteristics of NServiceBus but is designed never to overwhelm any of the worker nodes configured to receive work from it.

When to use it?
---------------

Scaling out (with or without a distributor) is only useful for where the work being done by a single machine takes time and therefore more computing resources helps. To help with this, monitor the [CriticalTime performance counter](monitoring-nservicebus-endpoints.md) on the endpoint and when you have the need, add in the distributor. Scaling out using the distributor when needed is made easy by not having to change code, just starting the same endpoint in distributor and worker profiles and this article explains how.

The distributor is applicable only when using Msmq as the transport for exchanging messages. NServiceBus uses MSMQ as the default transport. The distributor is not required when using other brokered transports like SqlServer, RabbitMQ, ActiveMQ, since they share the same queue, even if there are multiple instances of the endpoints running. NServiceBus will ensure that only one of these instances of that endpoint will process that message in this case.

Why use it?
-----------

When starting to use NServiceBus, you'll see that you can easily run multiple instances of the same process with the same input queue. This may look like scaling-out at first, but is really no different from running multiple threads within the same process. You'll see that you can't share a single input queue across multiple machines.

The distributor gets around this limitation.

What about MSMQ V4?
-------------------

Version 4 of MSMQ, made available with Vista and Server 2008, can perform 'remote transactional receive'. This means that processes on other machines can transactionally pull work from a queue on a different machine. If the machine processing the message crashes, the message roll back to the queue and other machines could then process it.

Even though the distributor provided similar functionality even before Vista was released, there are other reasons to use it even on the newer OS. The problem with 'remote transactional receive' is that it gets proportionally slower as more worker nodes are added. This is due to the overhead of managing more transactions, as well as the longer period of time that these transactions are open.

In short, the scale-out benefits of MSMQ V4 by itself are quite limited.

How does it work?
-----------------

Worker nodes send messages to the distributor, telling it when they're ready for work. These messages arrive at the distributor via a separate
'control' queue. The distributor stores this information. When applicative messages arrive at the distributor, it uses previously stored information to find a free worker node, and sends the message to it. If no worker nodes are free, the distributor waits before repeating the previous step.

All pending work stays in the distributor's queue (rather than building up in each of the workers' queues), giving visibility of how long messages are actually waiting. This is important for complying with time-based service level agreements (SLAs).

For more information on monitoring, see [Monitoring NServiceBus Endpoints](monitoring-nservicebus-endpoints.md) .

Where is it?
------------

Unlike NServiceBus V2.6, there is no specific Distributor process. Any NServiceBus endpoint can be configured to work as a Distributor.

To see the Distributor in action, see the [ScaleOut sample](scale-out-sample.md) .

Distributor configuration
-------------------------

### When hosting in NServiceBus.Host.exe

If you are running with [NServiceBus.Host.exe](the-nservicebus-host.md) , the following profiles start your endpoint with the Distributor functionality:

To start your endpoint as a distributor, run it from the command line, as follows:


    > NServiceBus.Host.exe NServiceBus.Production NServiceBus.Distributor


The NServiceBus.Distributor profile instructs the NServiceBus framework to start a distributor on this endpoint, waiting for workers to enlist to it. Unlike the NServiceBus.Master profile, the NServiceBus.Distributor profile does not execute a Worker on its node.

You can use the NServiceBus.Master to start a Distributor on your endpoint with a Worker on its endpoint. When specifying the NServiceBus.Master profile, the gateway also runs on endpoint.


    > NServiceBus.Host.exe NServiceBus.Production NServiceBus.Master


### When self-hosting

When you [self host](hosting-nservicebus-in-your-own-process.md) your endpoint, configure AsMasterNode() and then use this configuration:

-   RunDistributor(): Starts your endpoint as a Distributor, waits for
    Workers to enlist, and then distributes loads to those enlisted
    workers. It also starts a local worker (working from the same
    machine as the Distributor).
-   RunDistributorWithNoWorkerOnItsEndpoint(): Starts your endpoint as a
    Distributor, waiting for Workers to enlist, and then load balances.

Following is an example of a Distributor with a Worker on its endpoint:


```C#
Configure.With()
         .DefaultBuilder()
         .AsMasterNode()
         .RunDistributor()
         // Other settings go here
         .CreateBus()
         .Start();
```

 To run the Distributor without a worker on its endpoint, replace
.RunDistributor() with with .RunDistributorWithNoWorkerOnItsEndpoint().

Worker Configuration
--------------------

Any NServiceBus endpoint can run as a Worker node. To activate it, create a handler for the relevant messages and ensure that the app.config file contains routing information for the Distributor.

### When hosting in NServiceBus.Host.exe

If you are hosting your endpoint with NServiceBus.Host.exe, to run as a worker, use this command line:


    > NServiceBus.Host.exe NServiceBus.Production NServiceBus.Worker

Configure the name of the master node server as shown in this app.config example. Note the MasterNodeConfig section:



```XML
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <!-- Other sections go here -->
    <section name="MasterNodeConfig" type="NServiceBus.Config.MasterNodeConfig, NServiceBus.Core" />
  </configSections>
  <!-- Other config options go here -->
  <MasterNodeConfig Node="MachineWhereDistributorRuns"/>
</configuration>
```

 Read about the DistributorControlAddress and the DistributorDataAddress in the [Routing with the Distributor](load-balancing-with-the-distributor.md) section.

### When self-hosting

When self hosting, call the EnlistWithDistributor() configuration extension method. The Enlist command takes the endpoint name of the worker and sends "I am ready message" to the control queue of the distributor. See more in the Routing section below.

Following is an example of configuration of a Worker node:


```C#
Configure.With()
         .DefaultBuilder()
         .EnlistWithDistributor()
         // Other settings go here
         .CreateBus()
         .Start();
```

 Similar to self hosting, ensure the app.config of the worker contains the MasterNodeConfig section to point to the host name where the master node (and a distributor) are running.

Routing with the Distributor
----------------------------

The distributor uses two queues for its runtime operation. The DataInputQueue is the queue where the client processes send their applicative messages. The ControlInputQueue is the queue where the worker nodes send their control messages.

To use values other than the NServiceBus defaults you can override them, as shown in the UnicastBusConfig section below:


```XML
<UnicastBusConfig DistributorControlAddress="distributorControlBus@Cluster1" DistributorDataAddress="distributorDataBus@Cluster1">
  <MessageEndpointMappings>
    <!-- regular entries -->
  </MessageEndpointMappings>
</UnicastBusConfig>
```

 If those settings do not exist, the control queue is assumed as the endpoint name of the worker, concatenated with the " distributor.control@HostWhereDistributorIsRunning" string.

Similar to standard NServiceBus routing, you do not want high priority messages to get stuck behind lower priority messages, so just as you have separate NServiceBus processes for different message types, you also set up different distributor instances (with separate queues) for different message types.

In this case, name the queues just like the messages; for example, SubmitPurchaseOrder.StrategicCustomers.Sales. This is the name of the distributor's data queue and the input queues of each of the workers. The distributor's control queue is best named with a prefix of
'control', as follows: Control.SubmitPurchaseOrder.StrategicCustomers.Sales.

When using the distributor in a full publish/subscribe deployment, yousee is a distributor within each subscriber balancing the load of events being published, as follows:

![logical pub/sub and physical distribution 3](nservicebus_pubsub_3.png)

Keep in mind that the distributor is designed for load balancing within a single site, so do not use it between sites. In the image above, all publishers and subscribers are within a single physical site. For information on using NServiceBus across multiple physical sites, see
[the gateway](the-gateway-and-multi-site-distribution.md) .

High availability
-----------------

If the distributor goes down, even if its worker nodes remain running, they do not receive any messages. Therefore, it is important to run the distributor on a cluster that configuring its queues as clustered resources.

Since the distributor does not do CPU or memory intensive work, you can often put several distributor processes on the same clustered server. Be aware that the network IO may end up being the bottleneck for the distributor, so take into account message sizes and throughput when sizing your infrastructure.

Licensing and distribution
--------------------------

Basic [licensing](licensing-and-distribution.md) restricts the number of worker nodes to two.

Next steps
----------

Build a scalable solution using Master node and the workers solution that are in the [ScaleOut sample](scale-out-sample.md) .

