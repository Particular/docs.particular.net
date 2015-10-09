---
title: Howto deploy distributor and workers on a single machine for testing
summary: How to run the distributor and workers on a single machine for testing and demonstration purposes.
tags:
- Distributor
related:
- samples/scaleout
- nservicebus/scalability-and-ha/distributor
---

The default behavior of the distributor and workers is that each process shares the same queue names on the machine on which it is running. It is assumed that each instance runs on its own (virtual) machine.

## Easy: Use the master role

The distributor can be configured to be *both a distributor and worker* by using the *distributor master role*. 

### Using code

<!-- import DistributorMasterRoleCode -->

### Using NServiceBus.Host profile


### Version 4.3 and later

Using the separate nuget package [NServiceBus.Distributor.MSMQ](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ).


```cmd
NServiceBus.Host.exe NServiceBus.Master
```

### Before version 4.3

Using the embedded distributor.

```cmd
NServiceBus.Host.exe NServiceBus.MSMQMaster
```


## Hard: Use seperate instances

NServiceBus can be configured in such a way that you are able to run all instances on a single machine. Each instance will have its own set of queues.

Using this approach you will be able to kill a worker and see how the distributor behaves.


### Cloning the host

Each instance needs to run with its own customized configuration. The easiest way is to just clone the project multiple times and renaming the clones to identify that these are the workers.


### Distributor

The *Distributor* does not need any custom configuration. It is not aware of the workers, not at compile time or during deployment. Workers contact the distributor at runtime to let it know that it wants to participate in message distribution.


### Workers

Steps:

- Override the endpoint name so each instance gets their own set of queues
- Redirect message to correct queues for 
 - distributor control flow messages
 - timeout messages as only the distributor runs a timeout manager
- Override the worker input queue


#### Override endpoint name

The worker cannot share the same queues as the distributor. It needs its own set of queues. We need to override the endpoint name to achieve this and can only be done via code.

<!-- import Workerstartup -->


#### Redirect messages

The worker by default use the same queue names as the distributor but would run on a different (virtual) machine. It knows which machine to target as this is mentioned in the configuration.

<!-- import WorkerMasterNodeConfig -->


Overriding the endpoint name results in the worker to send distributor control and timeout message to incorrect queues. These queues need to be overriden in configuration. 

<!-- import WorkerTimeoutManagerAddress -->


### Override worker Input queue

Normally workers are deployed to different machines. When deployed to the same machine a GUID will be added to the end of the worker input queue name. This allows the distributor to properly route messages and prevents workers from competing on the same queue.

We need to be "faking different machines" and by using different instance configurations we can override the GUID behavior to prevent a proliferation of queue names.
 

#### Version 4 and lower

You need to hack the Local Address

<!-- import WorkerNameToUseWhileTestingCode -->


#### Version 5 and higher

You can use configuration 

<!-- import WorkerNameToUseWhileTestingConfig -->

## Sample

The [distributor scale out sample](/samples/scaleout/sample.md) is configured to run on a single machine.

