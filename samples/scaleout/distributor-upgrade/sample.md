---
title: Upgrading a Distributor endpoint from Version 5 to 6
summary: Upgrading an existing scaled-out endpoint that uses Distributor to version 6 of NServiceBus
reviewed: 2019-11-06
component: Core
related:
- transports/msmq/distributor
---

Note: This solution should be seen as a temporary solution. Support for the distributor is only available in Core version 6 and has been removed in core version 7 / msmq version 1. When all endpoints have been upgraded to version 6 it is advised to transition to [Sender Side Distribution](/transports/msmq/sender-side-distribution.md) in preparation for a future migration to core version 7 / msmq version 1.

## Initial state

This sample uses the following setup:

 * Sender sends commands to the scaled out endpoint.
 * Worker1 runs both distributor and a worker node.
 * Worker2 runs only a worker node.


## Upgrade

NServiceBus 6 does not have a distributor role. The distributor remains on NServiceBus 5.x, the workers can upgrade to 6.x.

Note: This sample solution contains 2 worker projects which is only done for demo purposes. A regular project would have a single project which would be deployed to several machines.


### Adding standalone distributor

Versions 6 and above no longer support running an embedded distributor so a separate project has to be added to the solution.

This new project references `NServiceBus` Version 5.X because the distributor does not have a Version 6 compatible release. The project also needs to reference the `NServiceBus.Distributor.MSMQ` package.


### Writing the distributor code

The following code is required to set up the distributor

snippet: DistributorCode

Notice that `false` is passed as a value for `withWorker` argument of the `RunMSMQDistributor` API call.


### Reconfiguring sender's routing

In the original sample the sender was sending messages to the master (Worker1). Now it needs to send messages to the new standalone distributor

snippet: SenderRouting


### Upgrading a worker

The `NServiceBus` package needs to be upgraded to 6.x. Workers no longer need the `NServiceBus.Distributor.MSMQ` package as the worker logic is embedded in the Version 6 package.

Note: Endpoints need to be upgraded to version 6 like any other endpoint. Read the [Upgrade Guides](/nservicebus/upgrades/) for additional information.


## Sample specific changes

### Emulating multiple workers

Note: The sample solution contains 2 workers for demo purposes. The second worker is carefully crafted so that the distributor sends messages to both workers where each worker has its own queue but on the same machine instead of multiple machines.

In Versions 6 and above each endpoint instance is identified by name of the endpoint and an ID of the instance. Both workers are going to be named `Samples.Scaleout.Worker` and the instance ID is going to be loaded from the app.config file. If the workers are deployed to separate machines the instance ID can be omitted.

snippet: WorkerIdentity


### Enlisting with the distributor

In Versions 6 and above there is a new API for enlisting with the distributor. The API requires passing addresses of distributor data and control queues which were previously read from the configuration section.

snippet: Enlisting

The last parameter, `capacity` is the maximum number of messages in-flight between the distributor and the worker. The worker sends an acknowledgement (ACK) message (also called `ready` message) for each message processed. The distributor keeps track of these ACKs to ensure no worker is flooded with work that it can't process

In Versions 5 and below the capacity was always set to the same value as the maximum concurrency setting, which means there were never more messages in-flight than threads ready to process them on the worker side. If the worker was processing it meant that messages never waited in the queue. This approach limits the potential consequences of worker failure at the expense of throughput.

A new setting was introduced in Version 6, which allows to explicitly control the capacity value. The snippet above shows how to set it to the default concurrency value of Version 6. If the endpoint configuration overrides this parameter with a custom value, an equal or larger value should be provided as `capacity` when enlisting with distributor.

### Upgrading the handlers

The handlers need to be upgraded to conform to new Version 6 API.


### Removing the shared code

Because the sender still uses Version 5 and the workers are on Version 6, the shared elements need to be moved to their target projects:

 * Copy the `ConfigErrorQueue` class to the `Sender` and `Distributor` projects.
 * Configure the `Sender` and `Worker` projects to use unobtrusive mode.


## Final state

After the upgrade is done both worker projects are identical (apart from the configuration file). The sender needed only a minor routing correction.

NOTE: When Version 5 nodes enlist with the distributor, they use GUID-based queues that are created each time the worker starts. This behavior is suppressed in the sample via a configuration switch. In Versions 6 and above when worker nodes enlists with the distributor, they always use their regular input queues with stable names.