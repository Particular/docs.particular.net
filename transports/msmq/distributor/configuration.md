---
title: Configuring Distributor and Workers
summary: Describes how to configure the distributor and its workers.
component: distributor
reviewed: 2017-06-30
tags:
 - Scalability
redirects:
 - nservicebus/msmq/distributor/configuration
---

## Versions

### NServiceBus 4.2 and below

In Version 4.2 and below Distributor was included in the NServiceBus package.

### NServiceBus 4.3 to 5.0

In Version 4.3 the built-in Distributor has been deprecated. The new dedicated [NServiceBus.Distributor.MSMQ](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ) package should be used instead.

### NServiceBus 6.0 and above

Running an embedded distributor process alongside the worker (also known as Master mode) is not supported in Versions 6 and higher. Instead, a stand alone Distributor endpoint running NServiceBus 5 should be used. 

Worker configuration sections like `MasterNodeConfig` are obsoleted and `endpointConfiguraiton.EnlistWithLegacyMSMQDistributor` should be used instead. For more information refer to [Upgrading a Distributor-based scaled out endpoint to Version 6](/samples/scaleout/distributor-upgrade/) documentation.

## Distributor configuration

### When hosting endpoints in NServiceBus.Host.exe

If running with [NServiceBus.Host.exe](/nservicebus/hosting/), the following profiles start the endpoint with the Distributor functionality:

partial: distributor

partial: master

### When self-hosting

When [self hosting](/nservicebus/hosting/) the endpoint, use this configuration:

snippet: ConfiguringDistributor

## Worker Configuration

Any NServiceBus endpoint can run as a Worker node. To activate it, create a handler for the relevant messages and ensure that the `app.config` file contains routing information for the Distributor.


### When hosting in NServiceBus.Host.exe

If hosting the endpoint with `NServiceBus.Host.exe`, to run as a Worker, use this command line:

partial: worker

Configure the name of the master node server as shown in this `app.config` example. Note the `MasterNodeConfig` section:

```xml
<configuration>
  <configSections>
    <!-- Other sections go here -->
    <section name="MasterNodeConfig" 
             type="NServiceBus.Config.MasterNodeConfig, NServiceBus.Core" />
  </configSections>
  <!-- Other config options go here -->
  <MasterNodeConfig Node="MachineWhereDistributorRuns"/>
</configuration>
```

Read about the `DistributorControlAddress` and the `DistributorDataAddress` in the [Routing with the Distributor](/transports/msmq/distributor/#routing-with-the-distributor) section.


### When self-hosting

If self-hosting the endpoint here is the code required to enlist the endpoint with a Distributor.

snippet: ConfiguringWorker

Similar to self-hosting, if running NServiceBus prior to Version 6, ensure the `app.config` of the worker contains the `MasterNodeConfig` section to point to the hostname where the distributor process is running.

## Troubleshooting

### Performance issues, throughput is low

#### Maximum concurrency

Make sure that the maximum concurrency level is increased on the distributor. By default, the maximum concurrency level is 1. Also, make sure that the workers have the maximum concurrency level set higher.

#### Prefetching

When your workers are using NServiceBus 6, then there is the ability to configure the worker capacity independently from the maximum concurrency. By assigning a capacity value higher then the maximum concurrency you are able to reduce the latency to start working on a next message as by default the worker first reports it is available for work and then gets a new message forwarded by the distributor.

#### Sender side distribution

Using [sender side distribution](h/transports/msmq/sender-side-distribution) instead of using the distributor might be a better alternative if you are able to use NServiceBus 6 and is much more suitable for scenarios where IO is the bottleneck. 

#### Do not run all distributors on a single box

Distributor endpoints are often deployed on *the distributor machine*. This is one machine that hosts all distributor endpoints. This becomes the bottleneck under load as all messages flow throught this machine. An alternative is to have multiple distributor machines on physically different networks interface cards.

Alternative, when first hosting all endpoints on a single machine do not use the distributor to now run *all* endpoints on multiple workers. First balance your current endpoints by moving endpoints. Eventually you will have only a single endpoint running on a machine. If this machine now becomes your bottleneck then you might first want to upscale this first and if upscaling becomes too costly then make use of the distributor. 

#### Upscaling

The distributor is very chatty. Network latency and bandwidth will lower the overal throughput when the CPU isn't your current bottleneck, using the distributor might not be the ideal solution as using the distributor will result in roughly 4 times the number of send messages thus has a severy impact on your network resources.


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
