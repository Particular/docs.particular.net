---
title: Deprecated Distributor in Version 6
tags:
 - upgrade
 - migration
 related:
 - samples/scaleout/distributor-upgrade
---

The [distributor](/nservicebus/scalability-and-ha/distributor.md) has been deprecated and replaced by [sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md) with NServiceBus Version 6.

## Client-side distribution

With sender-side distribution, senders are aware of scaled out receivers, allowing senders to distribute messages directly to the receivers instead of using an intermediary distributor.


## Upgrade scenario

This process aims to allow upgrade in production environments without missing events and minimal downtime.

WARN: To avoid duplicate events, the Distributor node needs to be upgraded to a worker node using NServiceBus 6 (it can be decommissioned after all workers are upgraded to Version 6).

* Upgrade all endpoints communicating to the Distributor (by commands or event subscription messages) to NServiceBus Version 6 first.
* Shut down the Distributor instance.
* [Upgrade the Distributor to NServiceBus Version 6](#upgrade-endpoint-to-version-6). This will migrate the Distributor endpoint to a regular worker instance.
* Start the instance again.
* [Upgrade the remaining worker nodes to NServiceBus Version 6](#upgrade-endpoint-to-version-6).
* Once all instances are upgraded to Version 6, configure all endpoints sending commands or receiving events from the Distributor to use [sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md) to send messages directly to the scaled out instances.


## Upgrade endpoint to Version 6

* Remove the `NServiceBus.Distributor.MSMQ` package.
* Remove Distributor specific configuration options.
  * Remove the `MasterNodeConfig` section from the application configuration file.
  * Remove the `DistributorControlAddress` and `DistributorDataAddress` attributes from the `UnicastBusConfig` configuration section in the application configuration file.
* Upgrade the endpoint to Version 6 (See the [Upgrade Guide](/nservicebus/upgrades/5to6)).


## Mixing NServiceBus Versions

Environments consisting entirely of NServiceBus Version 6 endpoints no longer require Distributor endpoints. NServiceBus Version 5 endpoints can communicate directly to NServiceBus Version 6 endpoints but the sender-side distribution feature of Version 6 is not compatible with endpoints using NServiceBus Version 5.

* NServiceBus Version 6 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 5 instances.
* NServiceBus Version 5 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 6 instances. In this scenario, the NServiceBus Version 6 instances need to [enlist with a NServiceBus Version 5 Distributor](#remove-subscriptions-for-the-distributor).
