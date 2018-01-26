---
title: Upgrading a Distributor endpoint from Version 5 to 6
summary: Upgrading an existing scaled-out endpoint that uses Distributor to version 6 of NServiceBus
reviewed: 2018-01-26
component: Core
tags:
- Scalability
related:
- transports/msmq/distributor
- samples/scaleout/distributor
---

## Initial state

This sample uses the same solution as the Version 5 [distributor sample](/samples/scaleout/distributor):

 * Sender sends commands to the scaled out endpoint.
 * Worker1 is a master (runs both distributor and a worker node).
 * Worker2 is a slave (runs only a worker node).


## Upgrade


### Adding stand-alone distributor

Versions 6 and above no longer support running an embedded distributor so a separate project has to be added to the solution.

This new project references `NServiceBus` Version 5.X because the distributor does not have a Version 6 compatible release. The project also needs to reference the `NServiceBus.Distributor.MSMQ` package.


### Writing the distributor code

The following code is required to set up the distributor

snippet: DistributorCode

Notice that `false` is passed as a value for `withWorker` argument of the `RunMSMQDistributor` API call.


### Reconfiguring sender's routing

In the original sample the sender was sending messages to the master (Worker1). Now it needs to send messages to the new stand-alone distributor

snippet: SenderRouting


### Upgrading the workers

As both the workers are now only processing messages, they no longer need the `NServiceBus.Distributor.MSMQ` package. The `NServiceBus` package needs to be upgraded to 6.x.

There are minor changes required to make the workers compile against the Version 6:

 * Changing `BusConfiguration` to `EndpointConfiguration`.
 * Using the new `async` endpoint create/start APIs.


### Giving workers new identity

In Versions 6 and above each endpoint instance is identified by name of the endpoint and an ID of the instance. Both workers are going to be named `Samples.Scaleout.Worker` and the instance ID is going to be loaded from the app.config file. If the workers are deployed to separate machines the instance ID can be omitted.

snippet: WorkerIdentity


### Enlisting with the distributor

In Versions 6 and above there is a new API for enlisting with the distributor. The API requires passing addresses of distributor data and control queues which were previously read from the configuration section.

snippet: Enlisting

The last parameter, `capacity` is the maximum number of messages in-flight between the distributor and the worker. The worker sends an acknowledgement (ACK) message (also called `ready` message) for each message processed. The distributor keeps track of these ACKs to ensure no worker is flooded with work that it can't process

In Versions 5 and below the capacity was always set to the same value as the maximum concurrency setting, which means there were never more messages in-flight than threads ready to process them on the worker side. If the worker was up and running messages never waited in the queue. This approach limits the potential consequences of worker failure at the expense of throughput.

A new setting was introduced in Version 6, which allows to explicitly control the capacity value. The snippet above shows how to set it to the default concurrency value of Version 6. If the endpoint configuration overrides this parameter with a custom value, an equal or larger value should be provided as `capacity` when enlisting with distributor.

### Upgrading the handlers

The handlers need to be upgraded to conform to new Version 6 API.


### Removing the shared code

Because the sender still uses Version 5 and the workers are on Version 6, the shared elements need to be moved to their target projects:

 * Copy the `ConfigErrorQueue` class to the `Sender` and `Distributor` projects.
 * Configure the `Sender` and `Worker` projects to use unobtrusive mode.


## Final state

After the upgrade is done both worker projects are identical (apart from the configuration file). The sender needed only a minor routing correction.

NOTE: When Version 5 nodes enlist with the distributor, they normally use a GUID-based queues that are created each time the worker starts. This behavior is suppressed in the sample via a configuration switch. In Versions 6 and above when worker nodes enlists with the distributor, they always use their regular input queues with stable names. 
