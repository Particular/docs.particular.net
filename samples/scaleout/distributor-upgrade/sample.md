---
title: Upgrading a Distributor-based scaled out endpoint to V6
summary: How to upgrade an existing scaled-out endpoint that uses Distributor to version 6 of NServiceBus
tags:
- Distributor
- Scalability
- Upgrade
related:
- nservicebus/scalability-and-ha
- samples/scaleout/distributor
---

## Initial state

This sample uses the same solution as the Version 5 [distributor sample](/samples/scaleout/distributor):

 * Sender sends commands to the scaled out endpoint.
 * Worker1 is a master (runs both distributor and a worker node).
 * Worker2 is a slave (runs only a worker node).


## Upgrade


### Adding stand-alone distributor

In version 6 NServiceBus no longer supports running an embedded distributor so a separate project has to be added to the solution. 

This new project is going to reference `NServiceBus` Version 5.X because distributor does not have a Version 6 compatible release. It also needs to reference the `NServiceBus.Distributor.MSMQ` package.


### Writing the distributor code

Following code is required to set up the distributor 

snippet:DistributorCode

Notice that `false` is passed as a value for `withWorker` argument of the `RunMSMQDistributor` API call.


### Reconfiguring sender's routing

In the original sample the sender was sending messages to the master (Worker1). Now it needs to send messages to the new stand-alone distributor

snippet:SenderRouting


### Upgrading the workers

As both the workers are now only processing messages, they no longer need the `NServiceBus.Distributor.MSMQ` package. The `NServiceBus` package needs to be upgraded to 6.x.

There are minor changes required to make the workers compile against the Version 6:

 * Changing `BusConfiguration` to `EndpointConfiguration`.
 * Using the new `async` endpoint create/start APIs.


### Giving workers new identity

In Version 6 each endpoint instance is identified by name of the endpoint and an ID of the instance. Both workers are going to be named `Samples.Scaleout.Worker` and the instance ID is going to be loaded from the app.config file. If the workers are deployed to separate machines the instance ID can be omitted.

snippet:WorkerIdentity


### Enlisting with the distributor

In Version 6 there is a new API for enlisting with the distributor. The API requires passing addresses of distributor data and control queues which were previously read from the configuration section.

snippet:Enlisting


### Upgrading the handlers

Handlers need to be upgraded to conform to new Version 6 API.


### Removing the shared code

Because the sender still uses Version 5 and the workers are on Version 6, the shared elements need to be moved to their target projects:

 * `ConfigErrorQueue` class copied to `Sender` and `Distributor` projects.
 * `Sender` and `Worker` projects configured to use unobtrusive mode.


## Final state

After the upgrade is done both worker projects are identical (apart from the configuration file). The sender needed only a minor routing correction.

NOTE: When Version 5 nodes enlist with the distributor, they use a GUID-based queues that are created each time the worker starts. This is different in Version 6. When Version 6 nodes enlists with the distributor, they use their regular input queues with stable names. 