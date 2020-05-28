---
title: MSMQ Distributor Troubleshooting
summary: Describes how to solve common issues with the distributor.
component: distributor
reviewed: 2019-11-05
related:
- transports/msmq/troubleshooting
---


## Troubleshooting

To scale out MSMQ processing, a Distributor node accepts messages in one queue and then forwards it to available workers. Every worker sends a `ReadyMessage` to the distributor *control queue* when it is ready to perform work and then the distributor forwards a message to that worker. This process is explained in detail in the [How the Distributor works](/transports/msmq/distributor/#how-the-distributor-works) section.

This troubleshooting guide covers issues related to MSMQ distributor only, and extends the [Troubleshooting MSMQ](/transports/msmq/troubleshooting.md) article. The problems with the MSMQ environment itself are the most common cause of distributor issues, for example due to worker's `ReadyMessages` getting stuck in the worker outgoing queue (unable to reach the distributor) or messages getting stuck in the distributor outgoing queue (unable to reach the worker).


### Performance issues

The distributor is not suitable for all scale-out scenarios, before using Distributor ensure that [it is appropriate in the given deployment setup](/transports/msmq/distributor/#performance).


#### Increase maximum concurrency

By default, the maximum concurrency level is 1. Ensure that the maximum concurrency level is increased on the distributor. Also, make sure that the workers have a higher maximum concurrency level.

Monitor server CPU, DISK, RAM and network resources to determine the optimal maximum concurrency level for real workloads.


#### Enable prefetching

When workers are using NServiceBus 6, then it is possible to configure the worker capacity independently from the maximum concurrency. By assigning a capacity value higher than the maximum concurrency it is possible to reduce the latency before the worker processes a next message. By default the worker first reports it is available for work and only then gets a new message forwarded by the distributor.


#### Scale up

If the CPU is not the bottleneck, then network latency and bandwidth will limit the overall throughput. Using the distributor might not be the ideal solution in such situation, as using the distributor will result in roughly 4 times the number of messages sent for each piece of work, thus having negative impact on the network resources.

Instead, in some situations it may be more appropriate to simply scale up hardware instead of using distributor and scale-out.


#### Verify network and disk congestion

Avoid running all distributor nodes on a single machine, evaluate other [distributor deployment configurations](/transports/msmq/distributor/#deployment-configurations).

If a distributor node cannot forward messages fast enough to the workers even though the workers are not fully utilizing available resources, then possible causes are that either the network bandwidth isn't sufficient or the disk used by MSMQ is too slow. 

Another alternative is using [sender side distribution](/transports/msmq/sender-side-distribution.md), which is recommended for scenarios where I/O is the bottleneck.


### SendLocal sends messages via the distributor

This is by design, when a worker uses `SendLocal` this means the message can be processed by any of the worker nodes. The backlog of messages on the worker nodes should be minimal and be at a maximum equal to the number of messages specified as its processing capacity.


### Each worker is getting events directly, bypassing the distributor 

The worker is very likely to not be running as a worker, but as a regular endpoint. In this configuration the worker will subscribe using its own address rather than using the distributor's address.

Make sure that:

- The assembly `NServiceBus.Distributor.MSMQ.dll` is in the endpoint folder.
- When using the [NServiceBus.Host](/nservicebus/hosting/nservicebus-host/), the `NServiceBus.MSMQWorker` profile is set on the command line and that the worker was installed with this argument on the commandline.
- When using [custom assembly scanning](/nservicebus/hosting/assembly-scanning.md), the `NServiceBus.Distributor.MSMQ.dll` assembly is not excluded.
- The log contains the following entry:
```  
Name: Distributor
Version: 5.0.5
Enabled by Default: No
Status: Enabled
Dependencies: None
Startup Tasks: None
```

### The distributor queue is building up while it is running

The distributor is probably unaware of any workers. When a worker starts, it signals its availability to the distributor and this event is logged:

```
INFO  NServiceBus.Distributor.MSMQ.MsmqWorkerAvailabilityManager Worker at 'Samples.Scaleout.Subscriber.Worker2@TUIN' is available to take on more work.
```

If the log doesn't contain such entries, then the worker is probably sending control messages to an incorrect distributor address.

Make sure that:

- In the **worker** `App.Config`:
  - The master node is correctly configured in the `MasterNodeConfig` configuration section.
  - The `DistributorDataAddress` and `DistributorControlAddress` attributed have the correct values in the `UnicastBusConfig` configuration section.
- The workers are running.
- The workers maximum concurrency level is higher than 1.

 
### The distributor is doing all the work

The distributor is likely not running as a distributor, but as a regular endpoint, or it is running in the *Master* configuration (distributor + worker) and isn't aware of any workers on any machines.

Make sure that:

- The assembly `NServiceBus.Distributor.MSMQ.dll` is in the endpoint folder.
- When using the NServiceBus.Host, that the worker has the `NServiceBus.MSMQWorker` profile on the command line and that the worker was installed with this argument on the command line.
- When using the NServiceBus.Host, that the distributor has either the `NServiceBus.MSMQDistributor` or `NServiceBus.MSMQMaster`
- When using custom assembly scanning, the `NServiceBus.Distributor.MSMQ.dll` assembly is not excluded.
- The log contains the following entry:
```  
Name: Distributor
Version: 5.0.5
Enabled by Default: No
Status: Enabled
Dependencies: None
Startup Tasks: None
```