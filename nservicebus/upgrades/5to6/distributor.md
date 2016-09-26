---
title: Migrating the Distributor to use Sender Side Distribution
tags:
 - upgrade
 - migration
related:
 - samples/scaleout/distributor-upgrade
---

The [distributor](/nservicebus/scalability-and-ha/distributor) has been deprecated and replaced by [sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md) with NServiceBus Version 6.

## Client-side distribution

With sender-side distribution, senders are aware of scaled out receivers, allowing senders to distribute messages directly to the receivers instead of using an intermediary distributor.


## General upgrade scenario

This process aims to allow upgrade in production environments without missing any messages and minimal downtime. If a scaled-out endpoint only receives commands and does not subscribe to messages consider following the [Simple upgrade scenario](#simple-upgrade-scenario) instead.

* Upgrade all endpoints that interact with the distributor to Version 6 first. At this stage, do not upgrade the workers to Version 6.
  * upgrade all endpoints that send command messages to the Distributor endpoint to be distributed,
  * upgrade all endpoints that send subscription messages to the Distributor, 
  * upgrade all endpoints subscribing to events published by worker nodes.
* Apply the following steps for each worker, one after another:
  * Shut down the worker.
  * [Upgrade to NServiceBus Version 6](#upgrade-endpoint-to-version-6).
  * [Configure it to enlist it with the distributor](#enlist-version-6-endpoints-with-a-distributor).
  * Start it again.
* Configure [sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md) for all endpoints sending commands or publishing events to the scaled out endpoint.
* Detach the workers from the Distributor by applying the following steps to the instances enlisted to the Distributor. But *skip this step for at least one instance* to ensure some workers remain attached to the distributor.
  * Shut down the worker.
  * Remove the `EnlistWithLegacyMSMQDistributor` configuration.
  * Start the worker again.

Most instances should now be detached from the Distributor. The Distributor can now either be upgraded to a regular worker node or be decommissioned:

### Migrate the Distributor to a worker node

This approach allows you to continue using the resources used for the Distributor as a regular worker node. This step only works when the Distributor endpoint has the same name as the worker node.

* Shut down the Distributor.
* Shut down the remaining instances attached to the Distributor.
* Replace the Distributor with a worker endpoint.
* Detach the attached instances from the Distributor by removing `EnlistWithLegacyMSMQDistributor` from the configuration.
* Start the Distributor and the detached instances again.
* Adjust the sender-side distribution configurations to include these endpoints.

### Decommission the Distributor

* [Manually remove the Distributor's subscriptions from publishing endpoints](#remove-distributor-subscriptions).
* Ensure no more messages are routed to the Distributor by updating routing and sender-side distribution configuration on sending endpoints.
* When the Distributor input queue is empty, shut down the Distributor.
* When the attached instances input queues are empty, shut down the attached instances.
* Remove the Distributor and the attached instances. Alternatively you can detach the attached instances from the Distributor and start them again (make sure to include them in the sender-side distribution configuration too).


## Simple upgrade scenario

This process describes a faster way to migrate a scaled-out endpoint using the Distributor to NServiceBus Version 6. This approach can only be applied if the endpoint does not subscribe to events and only handles incoming commands. Do not use this approach when endpoints use `Reply` to send messages back to workers attached to a Distributor.

WARNING: Following this process when endpoints subscribe to events may cause duplicate events or message loss!

* Upgrade all endpoints that interact with the distributor to Version 6 first. At this stage, do not upgrade the workers to Version 6.
  * upgrade all endpoints that send command messages to the Distributor endpoint to be distributed,
  * upgrade all endpoints that send subscription messages to the Distributor, 
  * upgrade all endpoints subscribing to events published by worker nodes.
* Configure all mentioned endpoints above to use [sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md) to route messages directly to the workers instead of the Distributor.
* Ensure no more messages are routed to the Distributor.
* Apply the following steps for each worker:
  * Shut down the worker.
  * [Upgrade to NServiceBus Version 6](#upgrade-endpoint-to-version-6).
  * Start it again.
* Shut down and remove the Distributor.


## Upgrade endpoint to Version 6

* Remove the `NServiceBus.Distributor.MSMQ` package.
* Remove Distributor specific configuration options.
  * Remove the `MasterNodeConfig` section from the application configuration file.
  * Remove the `DistributorControlAddress` and `DistributorDataAddress` attributes from the `UnicastBusConfig` configuration section in the application configuration file.
* Upgrade the endpoint to Version 6 (See the [Upgrade Guide](/nservicebus/upgrades/5to6).


## Remove Distributor subscriptions

When decommissioning the Distributor, it is necessary to manually remove remaining subscriptions in case the Distributor receives events to avoid issues on publishers or events being lost.

Follow the steps specified by the used subscription storage:


### Removing subscriptions from RavenDB persistence

For every endpoint the Distributor (including its workers) subscribes to:
* Find the publishers database in the RavenDB Management Studio.
* Go to the Query view.
* From the indexes select `Subscriptions`
* Enter the following Query: `Subscribers:*"<endpointAddress>"*` where you replace `<endpointAddress>` with the address of your endpoint. Make sure to escape special characters like `.` or `-` with `\`. E.g. `my\.endpoint@distributor\-machine`.
* Run the query.
* On all listed documents, remove the entry with the Distributor's subscriber address and save it.


### Removing subscriptions from NHibernate persistence

Execute the following script against the database which is configured for NHibernate persistence:

```
DELETE
FROM dbo.Subscription
WHERE SubscriberEndpoint = '<distributorAddress>'
```

where `<distributorAddress>` is the address of the Distributor. E.g. `My.Endpoint@distributor-machine`.


### Removing subscriptions from Azure Storage persistence

todo


## Use of the Distributor in Mixed Version Environments

Environments consisting entirely of NServiceBus Version 6 endpoints no longer require Distributor endpoints. NServiceBus Version 5 endpoints can communicate directly to NServiceBus Version 6 endpoints but the sender-side distribution feature of Version 6 is not compatible with endpoints using NServiceBus Version 5.

* NServiceBus Version 6 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 5 instances.
* NServiceBus Version 5 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 6 instances. In this scenario, the NServiceBus Version 6 instances need to [enlist with a NServiceBus Version 5 Distributor](#remove-subscriptions-for-the-distributor).

