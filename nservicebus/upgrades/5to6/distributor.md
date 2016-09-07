---
title: Deprecated Distributor in Version 6
tags:
 - upgrade
 - migration
 related:
 - samples/scaleout/distributor-upgrade
---

The [distributor](/nservicebus/scalability-and-ha/distributor.md) has been deprecated and replaced by [sender-side distribution]((/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md)) with NServiceBus Version 6.

## Client-side distribution

With sender-side distribution, senders are aware of scaled out receivers, allowing senders to distribute messages directly to the receivers instead of using an intermediary distributor.

## Upgrade scenarios

Based on your availability requirements, there are different approaches available to upgrade endpoints using the distributor to NServiceBus Version 6:

* Upgrade one instance at a time, allowing for high availability scenarios.
* Upgrade all endpoints communicating with the distributor at the same time.

See the following sections about a detailed description of the described approaches.


### One instance at a time

Use this process when shutting down all endpoints at once is no option. This process aims to allow upgrade in production environments at no downtime and without missing events.

* Upgrade all endpoints communicating to the distributor (by commands or event subscription messages) to NServiceBus Version 6 first.
* Apply the following steps for each scaled out instance enlisted to the Distributor, one instance after another:
  * Shut down the instance.
  * [Upgrade the instance to NServiceBus Version 6](#upgrade-endpoint-to-version-6).
  * [Enlist it with the distributor](#enlist-version-6-endpoints-with-a-distributor).
  * Start the instance again
* Once all instances are upgraded to Version 6, configure all endpoints communicating to the distributor (by commands or event subscription messages) to use [sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md) to send messages directly to the scaled out instances instead of the distributor.
* Detach some of the instances (but not all) from the Distributor by removing `endpointConfiguration.EnlistWithLegacyMSMQDistributor` and restarting them.
* [Unsubscribe the Distributor from all publishers](#remove-subscriptions-for-the-distributor)
* Verify now new messages are arriving at the Distributor's input queue. Make sure to give all publishers enough time update their routing tables, especially when using caching mechanisms.
* Once verified that no more messages are arriving at the Distributor, detach the remaining instances from the Distributor and restart them.
* Shut down the Distributor.


### All endpoints at once

This process requires all endpoints communicating with the Distributor to be shut down during the upgrade to ensure no commands or events are missed during the process.

* Shut down all endpoints communicating with the Distributor and it's instances first. This includes all endpoints sending commands, subscription messages to the Distributor and all endpoints receiving events from the Distributor's instances.
* Verify the Distributor's input queue is empty and no new messages are coming in.
* Shut down all scaled out endpoint instances enlisted to the Distributor.
* [Upgrade all endpoints to NServiceBus 6](#upgrade-endpoint-to-version-6).
* [Remove all subscriptions for the Distributor](#remove-subscriptions-for-the-distributor).
* Configure [sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md) for all endpoints communicating with the distributor (by commands or event subscription messages) to send messages directly to the scaled out instances instead of the distributor.
* Remove the distributor endpoint
* Start scaled out endpoint instances first.
* Start all other endpoints again.


## Upgrade steps

### Upgrade endpoint to Version 6
* Remove the `NServiceBus.Distributor.MSMQ` package.
* Remove Distributor specific configuration options.
  * Remove the `MasterNodeConfig` section from the application configuration file.
  * Remove the `DistributorControlAddress` and `DistributorDataAddress` attributes from the `UnicastBusConfig` configuration section in the application configuration file.
* Upgrade the endpoint to Version 6 (See the [Upgrade Guide](/nservicebus/upgrades/5to6)).


### Remove subscriptions for the distributor
//TODO


### Enlist Version 6 endpoints with a Distributor

Configure a NServiceBus Version 6 endpoint to enlist with NServiceBus Version 5 distributor using `endpointConfiguration.EnlistWithLegacyMSMQDistributor`:

```
busConfiguration.EnlistWithLegacyMSMQDistributor("Master@remoteMachine", "Master.Distributor.Control@remoteMachine", 1);
```


## Mixing NServiceBus Versions

Environments consisting entirely of NServiceBus Version 6 endpoints no longer require Distributor endpoints. NServiceBus Version 5 endpoints can communicate directly to NServiceBus Version 6 endpoints but the sender-side distribution feature of Version 6 is not compatible with endpoints using NServiceBus Version 5.

* NServiceBus Version 6 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 5 instances.
* NServiceBus Version 5 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 6 instances. In this scenario, the NServiceBus Version 6 instances need to [enlist with a NServiceBus Version 5 Distributor](#remove-subscriptions-for-the-distributor).
