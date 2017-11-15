---
title: MSMQ Distributor Troubleshooting
summary: Describes how to configure the distributor and its workers.
component: distributor
tags:
- Scalability
related:
- transports/msmq/troubleshooting
---

## Troubleshooting

To scale out MSMQ processing, a Distributor node accepts messages in one queue and then distributes/forwards it to eligible workers as they come available. This is accomplished by having each worker send a `ReadyMessage` to the distributor's *control queue* when it is ready for more work, and then the distributor forwards a message to that worker. [This process is explained in the distributor guidance](https://docs.particular.net/transports/msmq/distributor/#how-the-distributor-works).


This troubleshooting guide is specific to issues when using the MSMQ distributor, please read [Troubleshooting MSMQ](/transports/msmq/troubleshooting.md) too as it contains value information regarding MSMQ. These MSMQ environment problems are the leading cause of distributor issues, due to worker's `ReadyMessages` getting stuck in the workers' outgoing queues, unable to reach the distributor, or messages stuck in the distributor's outgoing queue, unable to reach the workers.


### Performance issues, throughput is low

Please make sure you understand the networking overhead. The distributor is not suitable for all scale-out scenarios. Please see:

- https://docs.particular.net/transports/msmq/distributor/#performance

#### Possibly increase Maximum concurrency

Make sure that the maximum concurrency level is increased on the distributor. By default, the maximum concurrency level is 1. Also, make sure that the workers have the maximum concurrency level set higher.

Monitor your server its CPU, DISK, RAM and network resources to find correct maximum concurrency level that suits the workload.

#### Enable prefetching

When your workers are using NServiceBus 6, then there is the ability to configure the worker capacity independently from the maximum concurrency. By assigning a capacity value higher then the maximum concurrency you are able to reduce the latency to start working on a next message as by default the worker first reports it is available for work and then gets a new message forwarded by the distributor.

#### Distributor causes network/disk congestion

Try to avoid running all distributor nodes on a single machine. Please read our guidance on [distributor deployment configurations](index.md#deploymentconfigurations).

If a distributor node cannot forward messages fast enough to the workers even though the workers are not hitting any limits then this could be caused because either the network bandwidth isn't sufficient or because the disk where MSMQ is stored on is too slow. An alternative might be using [sender side distribution](h/transports/msmq/sender-side-distribution) instead if you are able to use NServiceBus 6 and is much more suitable for scenarios where IO is the bottleneck where the distributor is more suited where the CPU is the bottleneck.


#### Up scaling

The distributor is very chatty. Network latency and bandwidth will lower the overal throughput when the CPU isn't your current bottleneck, using the distributor might not be the ideal solution as using the distributor will result in roughly 4 times the number of send messages thus has a severy impact on your network resources.


### SendLocal sends messages via the distributor

This is by design, when a worker uses `SendLocal` this means the message can be processed by any of the worker nodes. We want the backlog of messages on the worker nodes to be very minimal and be at max the number of messages specified as its processing capacity.


### Each worker is getting events directly, bypassing the distributor 

The worker is very likely to not be running as a worker, but as a regular endpoint. In this configuration the worker will subscribe using its own address and not using the distributor.

Make sure that:

- The assembly `NServiceBus.Distributor.MSMQ.dll` is in the endpoint folder.
- When using the [NServiceBus.Host](/nservicebus/hosting/nservicebus-host/), the `NServiceBus.MSMQWorker` profile is set on the commandline and that the worker was installed with this argument on the commandline.
- When using [custom assembly scanning](/nservicebus/hosting/assembly-scanning/), the in/exclusions are not excluding the `NServiceBus.Distributor.MSMQ.dll` assembly.
  - The log should have the following feature, and is easily found when searching on **Distributor**:
```  
Name: Distributor
Version: 5.0.5
Enabled by Default: No
Status: Enabled
Dependencies: None
Startup Tasks: None
```

### The distributor queue is building up while it is running

The distributor is probably unaware of any workers. When a worker starts it enlists its capacity at the distributor and this event is written to the log.

```
INFO  NServiceBus.Distributor.MSMQ.MsmqWorkerAvailabilityManager Worker at 'Samples.Scaleout.Subscriber.Worker2@TUIN' is available to take on more work.
```

If you do not see this log entry then the worker is probably sending the message to an incorrect distributor address.

Make sure that:

- In the **worker** `App.Config`:
  - The master node is correctly configured in the `MasterNodeConfig` configuration section.
  - The `DistributorDataAddress` and `DistributorControlAddress` attributed have the correct values in the `UnicastBusConfig` configuration section.
- The workers are running
- The workers might require a maximum concurrency level higher then 1 if your monitoring your resources are not indicating any bottlenecks.
 
### The distributor is doing all the work

The distributor is likely **not** running as the distributor, but as a regular endpoint, or it is running in the *Master* configuration (distributor + worker) and isn't aware of any workers on any machines.

Make sure that:

- The assembly `NServiceBus.Distributor.MSMQ.dll` is in the endpoint folder.
- When using the NServiceBus.Host, that the worker has the `NServiceBus.MSMQWorker` profile on the commandline and that the worker was installed with this argument on the commandline.
- When using the NServiceBus.Host, that the distributor has either the `NServiceBus.MSMQDistributor` or `NServiceBus.MSMQMaster`
- When using custom assembly scanning, the in/exclusions will not exclude the `NServiceBus.Distributor.MSMQ.dll` assembly.
  - The log should have the following feature, and is easily found when searching on **Distributor**:
```  
Name: Distributor
Version: 5.0.5
Enabled by Default: No
Status: Enabled
Dependencies: None
Startup Tasks: None
```
