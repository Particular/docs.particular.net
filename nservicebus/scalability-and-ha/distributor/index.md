---
title: Load Balancing with the Distributor
summary: The distributor maintains all the characteristics of NServiceBus but is designed never to overwhelm any of the worker nodes.
tags:
- Scalability
- Distributor
redirects:
 - nservicebus/load-balancing-with-the-distributor
---

The NServiceBus Distributor is similar in behavior to standard [load balancers](https://en.wikipedia.org/wiki/Load_balancing_%28computing%29). It is the key to transparently scaling out message processing over many machines.

As a standard NServiceBus process, the Distributor maintains all the fault-tolerant and performance characteristics of NServiceBus but is designed never to overwhelm any of the worker nodes configured to receive work from it.


## When to use it?

Scaling out (with or without a Distributor) is only useful for where the work being done by a single machine takes time and therefore more computing resources helps. To help with this, monitor the [CriticalTime performance counter](/nservicebus/operations/performance-counters.md) on the endpoint and when you have the need, add in the Distributor. Scaling out using the Distributor when needed is made easy by not having to change code, just starting the same endpoint in Distributor and Worker profiles and this article explains how.

The Distributor is applicable only when using MSMQ as the transport for exchanging messages. NServiceBus uses MSMQ as the default transport. The Distributor is not required when using other brokered transports like SqlServer and RabbitMQ, since they share the same queue, even if there are multiple instances of the endpoints running. NServiceBus will ensure that only one of these instances of that endpoint will process that message in this case.

WARNING: Keep in mind that the Distributor is designed for load balancing within a single site, so do not use it between sites. In the image above, all publishers and subscribers are within a single physical site. For information on using NServiceBus across multiple physical sites, see [the gateway](/nservicebus/gateway/multi-site-deployments.md).


## Why use it?

When starting to use NServiceBus, you'll see that you can easily run multiple instances of the same process with the same input queue. This may look like scaling-out at first, but is really no different from running multiple threads within the same process. You'll see that you can't share a single input queue across multiple machines.

The Distributor gets around this limitation.


## What about MSMQ Version 4?

Version 4 of MSMQ, made available with Vista and Server 2008, can perform [remote transactional receive](https://msdn.microsoft.com/en-us/library/ms700128.aspx). This means that processes on other machines can transactionally pull work from a queue on a different machine. If the machine processing the message crashes, the message roll back to the queue and other machines could then process it.

Even though the Distributor provided similar functionality even before Vista was released, there are other reasons to use it even on the newer operating systems. The problem with 'remote transactional receive' is that it gets proportionally slower as more worker nodes are added. This is due to the overhead of managing more transactions, as well as the longer period of time that these transactions are open.

In short, the scale-out benefits of MSMQ Version 4 by itself are quite limited.


## Performance

For each message being processed, the Distributor performs a few additional operations: it receives a ready message from a Worker, sends the work message to the Worker and receives a ready message post processing. That means that using Distributor introduces a certain processing overhead, that is independent of how much actual work is done. Therefore the Distributor is more suitable for relatively long running units of work (high I/O like http calls, writing to disk) as opposed to very short lived units of work (a quick read from the database and dispatching a message using `Bus.Send` or  `Bus.Publish`).

To get a sense of the expected performance take your regular endpoint performance and divide it by 4.

If you need to scale out small units of work you might consider splitting your handlers into smaller vertical slices of functionality and deploying them on their own endpoints.


## How does it work?

Worker nodes send messages to the Distributor, telling it when they're ready for work. These messages arrive at the distributor via a separate 'control' queue:

![worker registration](how-distributor-works-1.png)

Then the distributor creates a ready message per available thread:

![worker registration](how-distributor-works-2.png)

The distributor stores this information. When a message arrives at the Distributor, it uses previously stored information to find a free Worker node, and sends the message to it. If no Worker nodes are free, the Distributor waits before repeating the previous step.

![worker registration](how-distributor-works-3.png)

![worker registration](how-distributor-works-4.png)


All pending work stays in the Distributor's queue (rather than building up in each of the Workers' queues), giving visibility of how long messages are actually waiting. This is important for complying with time-based service level agreements (SLAs).

For more information on monitoring, see [Performance Counters](/nservicebus/operations/performance-counters.md).

For more information about Pub/Sub in a distributor scenario see [What the distributor does](/nservicebus/messaging/publish-subscribe/#what-the-distributor-does) and [The same for any publisher node](/nservicebus/messaging/publish-subscribe/#the-same-for-any-publisher-node)


## Distributor configuration

NOTE: As of Version 6 NServiceBus no longer supports running an embedded distributor. Because of that configuration sections like `MasteNodeConfig` are obsoleted. Read more about [Upgrading a Distributor-based scaled out endpoint to V6](/samples/scaleout/distributor-upgrade/)

### When hosting endpoints in NServiceBus.Host.exe

If you are running with [NServiceBus.Host.exe](/nservicebus/hosting/), the following profiles start the endpoint with the Distributor functionality:

To start the endpoint as a Distributor ensure you install the [NServiceBus.Distributor.MSMQ NuGet](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ) and then run the host from the command line, as follows:

```cmd
NServiceBus.Host.exe NServiceBus.MSMQDistributor
```
or if using a version of NServiceBus that is earlier than Version 4.3:
```cmd
NServiceBus.Host.exe NServiceBus.Distributor
```

The NServiceBus.[MSMQ]Distributor profile instructs the NServiceBus framework to start a Distributor on this endpoint, waiting for workers to enlist to it. Unlike the NServiceBus.[MSMQ]Master profile, the NServiceBus.[MSMQ]Distributor profile does not execute a Worker on its node.

You can use the NServiceBus.[MSMQ]Master to start a Distributor on the endpoint with a Worker on its endpoint.
To start the endpoint as a Master ensure you install the [NServiceBus.Distributor.MSMQ NuGet](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ) and then run the host from the command line, as follows:
```cmd
NServiceBus.Host.exe NServiceBus.MSMQMaster
```
or if using a version of NServiceBus that is earlier than Version 4.3:
```cmd
NServiceBus.Host.exe NServiceBus.Master
```


### When self-hosting

When you [self host](/nservicebus/hosting/) the endpoint, use this configuration:

snippet:ConfiguringDistributor

NOTE: In versions 4 and above the sample above is using [NServiceBus.Distributor.MSMQ NuGet](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ).


## Worker Configuration

Any NServiceBus endpoint can run as a Worker node. To activate it, create a handler for the relevant messages and ensure that the `app.config` file contains routing information for the Distributor.


### When hosting in NServiceBus.Host.exe

If you are hosting your endpoint with NServiceBus.Host.exe, to run as a Worker, use this command line:

```cmd
NServiceBus.Host.exe NServiceBus.MSMQWorker
```

or if using a version of NServiceBus that is earlier than Version 4.3:

```cmd
NServiceBus.Host.exe NServiceBus.Worker
```

Configure the name of the master node server as shown in this `app.config` example. Note the `MasterNodeConfig` section:

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

Read about the `DistributorControlAddress` and the `DistributorDataAddress` in the [Routing with the Distributor](#routing-with-the-distributor) section.


### When self-hosting

If you are self-hosting the endpoint here is the code required to enlist the endpoint with a Distributor.

snippet: ConfiguringWorker

NOTE: In versions 4 and above the sample above is using [NServiceBus.Distributor.MSMQ NuGet](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ).

Similar to self hosting, ensure the `app.config` of the Worker contains the `MasterNodeConfig` section to point to the host name where the master node (and a Distributor) are running.


## Is enabled in endpoint

For some extensibility scenarios it may be helpful to check if the endpoint is running as either a worker, a distributor, or both.


### Is running as a Distributor

snippet: IsDistributorEnabled


### Is running as a Worker

snippet: IsWorkerEnabled


## Routing with the Distributor

The Distributor uses two queues for its runtime operation. The `DataInputQueue` is the queue where the client processes send their applicable messages. The `ControlInputQueue` is the queue where the worker nodes send their control messages.

To use values other than the NServiceBus defaults you can override them, as shown in the `UnicastBusConfig` section below:

```XML
<UnicastBusConfig DistributorControlAddress="EndpointName.Distributor.Control@MachineWhereDistributorRuns" DistributorDataAddress="EndpointName@MachineWhereDistributorRuns">
  <MessageEndpointMappings>
    <!-- regular entries -->
  </MessageEndpointMappings>
</UnicastBusConfig>
```

If those settings do not exist, the control queue is assumed as the endpoint name of the worker, concatenated with the `distributor.control@HostWhereDistributorIsRunning` string.

Similar to standard NServiceBus routing, you do not want high priority messages to get stuck behind lower priority messages, so just as you have separate NServiceBus processes for different message types, you also set up different Distributor instances (with separate queues) for different message types.

In this case, name the queues just like the messages. For example, `SubmitPurchaseOrder.StrategicCustomers.Sales`. This is the name of the distributor's data queue and the input queues of each of the workers.


## Worker QMId needs to be unique

Every installation of MSMQ on a Windows machine is represented uniquely by a Queue Manager ID (QMId). The QMId is stored as a key in the registry, `HKLM\Software\Microsoft\MSMQ\Parameters\Machine Cache`. MSMQ uses the QMId to know where is should send acks and replies for incoming messages.

It is very important that all your machines have their own unique QMId. If two or more machines share the same QMId, only one of those machines are able so successfully send and receive messages with MSMQ. Exactly which machine works changes in a seemingly random fashion.

The primary reason for machines ending up with duplicate QMIds is cloning of virtual machines from a common Windows image without running the recommended [Sysprep](https://technet.microsoft.com/en-us/library/cc766049.aspx) tool.

Should you have two or more machines with the same QMId reinstall the MSMQ feature to generate a new QMId.

Check out [John Breakwell's blog](http://blogs.msdn.com/b/johnbreakwell/archive/2007/02/06/msmq-prefers-to-be-unique.aspx) for more details.


## High availability

If the Distributor goes down, even if its worker nodes remain running, they do not receive any messages. It is important to run the Distributor on a cluster that has its its queues configured as clustered resources.

Since the Distributor does not do CPU or memory intensive work, you can often put several Distributor processes on the same clustered server. Be aware that the network IO may end up being the bottleneck for the Distributor, so take into account message sizes and throughput when sizing your infrastructure.