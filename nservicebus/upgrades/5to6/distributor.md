---
title: Migrating the Distributor to use Sender Side Distribution
reviewed: 2016-10-25
related:
 - samples/scaleout/distributor-upgrade
redirects:
 - nservicebus/scalability-and-ha/distributor/upgrading-the-distributor
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

The [distributor](/transports/msmq/distributor) has scaling limitations. For better scalability the distributor should be replaced by [sender-side distribution](/transports/msmq/sender-side-distribution.md) with NServiceBus Version 6. Before upgrading, consider the current [limitations of sender-side distribution](/transports/msmq/sender-side-distribution.md#limitations) mode.


## Client-side distribution

With sender-side distribution, senders are aware of scaled out receivers, allowing senders to distribute messages directly to the receivers instead of using an intermediary distributor.


## General upgrade scenario

This process aims to allow upgrade without message loss and minimal downtime. If a scaled-out endpoint only receives commands and does not subscribe to messages consider following the [Simple upgrade scenario](#simple-upgrade-scenario) instead.

 * Upgrade all endpoints that interact with the distributor to Version 6 first. At this stage, do not upgrade the workers to Version 6.
  * Upgrade all endpoints that send command messages to the Distributor endpoint to be distributed,
  * Upgrade all endpoints that send subscription messages to the Distributor,
  * Upgrade all endpoints subscribing to events published by worker nodes.
 * Apply the following steps for each worker, one after another:
  * Shut down the worker.
  * [Upgrade to NServiceBus Version 6](#upgrade-endpoint-to-version-6).
  * [Configure it to enlist it with the distributor](/transports/msmq/distributor/configuration.md#worker-configuration-when-self-hosting).
  * Start the worker again.
 * Configure [sender-side distribution](/transports/msmq/sender-side-distribution.md) for all endpoints sending commands or publishing events to the scaled out endpoint. Leave one instance of the scaled-out endpoint excluded from the sender-side distribution for now.
 * Detach the workers from the Distributor by applying the following steps to the instances enlisted to the Distributor. But **skip this step for the instance that was not included in the sender-side distribution** to ensure the distributor queue can be drained.
  * Shut down the worker.
  * Remove the `EnlistWithLegacyMSMQDistributor` configuration.
  * Start the worker again.

Most instances should now be detached from the Distributor. The Distributor can now either be upgraded to a regular worker node or be decommissioned:


### Migrate the Distributor to a worker node

This approach enables the continued utilization of the resources used for the Distributor as a regular worker node. This step only works when the Distributor endpoint has the same name as the worker node.

 * Shut down the Distributor.
 * Shut down the remaining instances attached to the Distributor.
 * Replace the Distributor with a worker endpoint.
 * Detach the attached instances from the Distributor by removing `EnlistWithLegacyMSMQDistributor` from the configuration.
 * Start the Distributor and the detached instances again.
 * Adjust the sender-side distribution configurations to include these endpoints.


### Decommission the Distributor

 * [Manually remove the Distributor's subscriptions from publishing endpoints](#remove-distributor-subscriptions).
 * Ensure no more messages are routed to the Distributor by updating routing and sender-side distribution configuration on sending endpoints.
 * Ensure there are no delayed messages (e.g. [delayed retries](/nservicebus/recoverability/#delayed-retries)) pending on the Distributor queue. This needs to be checked manually on the selected persitence option.
 * When the Distributor input queue is empty, shut down the Distributor.
 * When the attached instances input queues are empty, shut down the attached instances.
 * Remove the Distributor and the attached instances. Alternatively, detach the attached instances from the Distributor and start them again (make sure to include them in the sender-side distribution configuration too).

Finally, include the previously excluded instance in the sender-side distribution configuration.


## Simple upgrade scenario

This process describes a faster way to migrate a scaled-out endpoint using the Distributor to NServiceBus Version 6. This approach can only be applied if the endpoint does not subscribe to events and only handles incoming commands. Do not use this approach when endpoints use `Reply` to send messages back to workers attached to a Distributor.

DANGER: Following this process when endpoints subscribe to events may cause duplicate events or message loss.

 * Upgrade all endpoints that interact with the distributor to Version 6 first. At this stage, do not upgrade the workers to Version 6.
  * Upgrade all endpoints that send command messages to the Distributor endpoint to be distributed,
  * Upgrade all endpoints that send subscription messages to the Distributor,
  * Upgrade all endpoints subscribing to events published by worker nodes.
 * Configure all mentioned endpoints above to use [sender-side distribution](/transports/msmq/sender-side-distribution.md) to route messages directly to the workers instead of the Distributor.
 * Ensure no more messages are routed to the Distributor.
 * Apply the following steps for each worker:
  * Shut down the worker.
  * [Upgrade to NServiceBus Version 6](#upgrade-endpoint-to-version-6).
  * Start the worker again.
 * Shut down and remove the Distributor.


## Upgrade endpoint to Version 6

 * Remove the `NServiceBus.Distributor.MSMQ` package.
 * Remove Distributor specific configuration options.
  * Remove the `MasterNodeConfig` section from the application configuration file.
  * Remove the `DistributorControlAddress` and `DistributorDataAddress` attributes from the `UnicastBusConfig` configuration section in the application configuration file.
 * Upgrade the endpoint to NServiceBus Version 6 (See the [Upgrade Guide](/nservicebus/upgrades/5to6).


## Remove Distributor subscriptions

When decommissioning the Distributor, it is necessary to manually remove remaining subscriptions in case the Distributor receives events to avoid issues on publishers or events being lost.

Follow the steps specified by the used subscription storage:


### Removing subscriptions from [RavenDB Persistence](/persistence/ravendb)

Before manually modifying documents stored in RavenDB, make sure to create a [Database Backup](https://ravendb.net/docs/search/latest/csharp?searchTerm=backup).

For every endpoint the Distributor (including its workers) subscribes to:

 * Find the publishers database in the [RavenDB Management Studio](https://ravendb.net/docs/search/latest/csharp?searchTerm=management-studio).
 * Go to the [Query View](https://ravendb.net/docs/search/latest/csharp?searchTerm=query%20view).
 * From the indexes select `Subscriptions`
 * Enter the following Query: `Subscribers:*"<endpointAddress>"*` and replace `<endpointAddress>` with the address of the distributor endpoint. Make sure to escape special characters like `.` or `-` with `\`. E.g. `my\.endpoint@distributor\-machine`.
 * Run the query.
 * On all listed documents, remove the entry with the Distributor's subscriber address and save it.

See the following sample documents on how to remote the Distributor subscription:

Before:

```javascript
{
    "MessageType": "Shared.Events.DemoEvent, Version=1.0.0.0",
    "Subscribers": [
        {
            "TransportAddress": "Samples.Subscriber@machineA",
            "Endpoint": "Samples.Subscriber"
        },
        {
            "TransportAddress": "Samples.Distributor@distributor",
            "Endpoint": "Samples.Distributor"
        }
    ]
}
```

After:

```javascript
{
    "MessageType": "Shared.Events.DemoEvent, Version=1.0.0.0",
    "Subscribers": [
        {
            "TransportAddress": "Samples.Subscriber@machineA",
            "Endpoint": "Samples.Subscriber"
        }
    ]
}
```


### Removing subscriptions from [NHibernate Persistence](/persistence/nhibernate)

Remove the Distributor from publisher's subscription storages by removing all subscriptions related to the Distributor's address. See [NHibernate Persistence Scripting](/persistence/nhibernate/scripting.md) on how to remove subscriptions.


### Removing subscriptions from [Azure Storage Persistence](/persistence/azure-storage)

Remove the Distributor from publisher's subscription storages by removing all subscriptions related to the Distributor's address. See [Azure Storage Persistence Scripting](/persistence/azure-storage/scripting.md) on how to remove subscriptions.


## Use of the Distributor in Mixed Version Environments

Environments consisting entirely of NServiceBus Version 6 endpoints no longer require Distributor endpoints. NServiceBus Version 5 endpoints can communicate directly to NServiceBus Version 6 endpoints but the sender-side distribution feature of Version 6 is not compatible with endpoints using NServiceBus Version 5.

 * NServiceBus Version 6 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 5 instances.
 * NServiceBus Version 5 endpoints need to route their messages to a Distributor when communicating with scaled out NServiceBus Version 6 instances. In this scenario, the NServiceBus Version 6 instances need to [enlist with a NServiceBus Version 5 Distributor](/transports/msmq/distributor/configuration.md#worker-configuration-when-self-hosting).
