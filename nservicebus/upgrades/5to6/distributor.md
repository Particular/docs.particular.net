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


## Upgrade scenario

This process aims to allow upgrade in production environments without missing any messages and minimal downtime.

* Upgrade all endpoints that interact with the distributor to Version 6 first. For example, upgrade all endpoints that send command messages to the Distributor endpoint for the load to be distributed, or endpoints that send subscription messages to the distributor, subscribing to events when a worker node publishes an event. At this stage, do not upgrade the workers to Version 6.
* Apply the following steps for each workr, one after another:
  * Shut down.
  * [Upgrade to NServiceBus Version 6](#upgrade-endpoint-to-version-6).
  * [Configure it to enlist it with the distributor](#enlist-version-6-endpoints-with-a-distributor).
  * Start it again.
* Configure [sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md) for all endpoints sending commands or publishing events to the scaled out endpoint.
* Detach the instances from the Distributor by applying the following steps to the instances enlisted to the Distributor. But *skip this step for at least one instance* to ensure some instances are still attached to the distributor.
  * Shut down the instance.
  * Detach the instance from the Distributor by removing the `EnlistWithLegacyMSMQDistributor` configuration.
  * Start the instance again.

Most instances should now be detached from the Distributor. The Distributor can now either be upgraded to a regular worker node or be decommissioned:

### Upgrade the Distributor to a worker node

This approach allows you to continue using the resources used for the Distributor as a regular worker node.

* Endpoint need to have the same endpoint name as other workers. Change queue somehow?

* Shut down the Distributor.
* Shut down the remaining instances attached to the Distributor.
* Replace the Distributor with a worker endpoint.
* Detach the attached instances from the Distributor.
* Start the Distributor and the detached instances again.
* Adjust the sender-side distribution configuration to include these endpoints.

### Decommission the Distributor


* [Manually remove the Distributor's subscriptions from publishing endpoints](todo).
* Ensure no more messages are routed to the Distributor by updating routing and sender-side distribution configuration on sending endpoints.
* When the Distributor input queue is empty, shut down the Distributor.
* When the attached instances input queues are empty, shut down the attached instances.
* Remove the Distributor and the attached instances. Alternatively you can detach the attached instances from the Distributor and start them again (Make sure to include them in the sender-side distribution configuration too)


## Upgrade endpoint to Version 6

* Remove the `NServiceBus.Distributor.MSMQ` package.
* Remove Distributor specific configuration options.
  * Remove the `MasterNodeConfig` section from the application configuration file.
  * Remove the `DistributorControlAddress` and `DistributorDataAddress` attributes from the `UnicastBusConfig` configuration section in the application configuration file.
* Upgrade the endpoint to Version 6 (See the [Upgrade Guide](/nservicebus/upgrades/5to6).


## Remove Distributor subscriptions

When decomissioning the Distributor, it is necessary to manually remove remaining subscripions in case the Distributor receives events to avoid issues on publishers or events being lost.

Follow the steps specified by the used subscription storage:


### Removing subscriptions from RavenDB persistence

todo


### Removing subscriptions from NHibernate persistence

todo


### Removing subscriptions from Azure Storage persistence

todo


## Use of the Distributor in Mixed Version Environments

Environments consisting entirely of NServiceBus Version 6 endpoints no longer require Distributor endpoints. NServiceBus Version 5 endpoints can communicate directly to NServiceBus Version 6 endpoints but the sender-side distribution feature of Version 6 is not compatible with endpoints using NServiceBus Version 5.

* NServiceBus Version 6 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 5 instances.
* NServiceBus Version 5 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 6 instances. In this scenario, the NServiceBus Version 6 instances need to [enlist with a NServiceBus Version 5 Distributor](#remove-subscriptions-for-the-distributor).
