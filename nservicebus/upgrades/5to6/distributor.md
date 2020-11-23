---
title: Migrating the distributor to use sender-side distribution
reviewed: 2020-04-30
related:
 - samples/scaleout/distributor-upgrade
redirects:
 - nservicebus/scalability-and-ha/distributor/upgrading-the-distributor
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

The [distributor](/transports/msmq/distributor) has scaling limitations. For better scalability, the distributor should be replaced by [sender-side distribution](/transports/msmq/sender-side-distribution.md) with NServiceBus version 6. Before upgrading, consider the current [limitations of sender-side distribution](/transports/msmq/sender-side-distribution.md#limitations) mode.


## Client-side distribution

With sender-side distribution, senders are aware of scaled-out receivers, allowing senders to distribute messages directly to the receivers instead of using an intermediary distributor.


## General upgrade scenario

This process aims to allow upgrade without message loss and with minimal downtime. If a scaled-out endpoint only receives commands and does not subscribe to messages, consider following the [simple upgrade scenario](#simple-upgrade-scenario) instead.

 * Upgrade all endpoints that interact with the distributor to version 6 first. At this stage, do not upgrade the workers to version 6.
  * Upgrade all endpoints that send command messages to the distributor endpoint to be distributed.
  * Upgrade all endpoints that send subscription messages to the distributor.
  * Upgrade all endpoints subscribing to events published by worker nodes.
 * Apply the following steps for each worker, one after another:
  * Shut down the worker.
  * [Upgrade to NServiceBus version 6](#upgrade-endpoint-to-version-6).
  * [Configure it to enlist it with the distributor](/transports/msmq/distributor/configuration.md#worker-configuration-when-self-hosting).
  * Start the worker again.
 * Configure [sender-side distribution](/transports/msmq/sender-side-distribution.md) for all endpoints sending commands or publishing events to the scaled-out endpoint. Leave one instance of the scaled-out endpoint excluded from the sender-side distribution for now.
 * Detach the workers from the distributor by applying the following steps to the instances enlisted to the distributor. But **skip this step for the instance that was not included in the sender-side distribution** to ensure the distributor queue can be drained.
  * Shut down the worker.
  * Remove the `EnlistWithLegacyMSMQDistributor` configuration.
  * Start the worker again.

Most instances should now be detached from the distributor. The distributor can now either be upgraded to a regular worker node or be decommissioned:


### Migrate the distributor to a worker node

This approach enables the continued utilization of the resources used for the distributor as a regular worker node. This step only works when the distributor endpoint has the same name as the worker node.

 * Shut down the distributor.
 * Shut down the remaining instances attached to the distributor.
 * Replace the distributor with a worker endpoint.
 * Detach the attached instances from the distributor by removing `EnlistWithLegacyMSMQDistributor` from the configuration.
 * Start the distributor and the detached instances again.
 * Adjust the sender-side distribution configurations to include these endpoints.


### Decommission the distributor

 * [Manually remove the distributor's subscriptions from publishing endpoints](#remove-distributor-subscriptions).
 * Ensure no more messages are routed to the distributor by updating routing and sender-side distribution configuration on sending endpoints.
 * Ensure there are no delayed messages (e.g. [delayed retries](/nservicebus/recoverability/#delayed-retries)) pending on the distributor queue. This must be checked manually on the selected persitence option.
 * When the distributor input queue is empty, shut down the distributor.
 * When the attached instances input queues are empty, shut down the attached instances.
 * Remove the distributor and the attached instances. Alternatively, detach the attached instances from the distributor and start them again (make sure to include them in the sender-side distribution configuration too).

Finally, include the previously excluded instance in the sender-side distribution configuration.


## Simple upgrade scenario

This process describes a faster way to migrate a scaled-out endpoint using the distributor to NServiceBus version 6. This approach can be applied only if the endpoint does not subscribe to events and only handles incoming commands. Do not use this approach when endpoints use `Reply` to send messages back to workers attached to a distributor.

DANGER: Following this process when endpoints subscribe to events may cause duplicate events or message loss.

 * Upgrade all endpoints that interact with the distributor to version 6 first. At this stage, do not upgrade the workers to version 6.
  * Upgrade all endpoints that send command messages to the distributor endpoint to be distributed.
  * Upgrade all endpoints that send subscription messages to the distributor.
  * Upgrade all endpoints subscribing to events published by worker nodes.
 * Configure all mentioned endpoints above to use [sender-side distribution](/transports/msmq/sender-side-distribution.md) to route messages directly to the workers instead of the distributor.
 * Ensure no more messages are routed to the distributor.
 * Apply the following steps for each worker:
  * Shut down the worker.
  * [Upgrade to NServiceBus version 6](#upgrade-endpoint-to-version-6).
  * Start the worker again.
 * Shut down and remove the distributor.


## Upgrade endpoint to version 6

 * Remove the `NServiceBus.Distributor.MSMQ` package.
 * Remove distributor-specific configuration options.
  * Remove the `MasterNodeConfig` section from the application configuration file.
  * Remove the `DistributorControlAddress` and `DistributorDataAddress` attributes from the `UnicastBusConfig` configuration section in the application configuration file.
 * Upgrade the endpoint to NServiceBus version 6. (See the [upgrade guide](/nservicebus/upgrades/5to6).


## Remove distributor subscriptions

When decommissioning the distributor, it is necessary to manually remove remaining subscriptions in case the distributor receives events to avoid issues on publishers or events being lost.

Follow the steps specified by the used subscription storage:


### Removing subscriptions from [RavenDB persistence](/persistence/ravendb)

Before manually modifying documents stored in RavenDB, make sure to create a [database backup](https://ravendb.net/docs/search/latest/csharp?searchTerm=backup).

For every endpoint the distributor subscribes to (including its workers):

 * Find the publishers database in [RavenDB Management Studio](https://ravendb.net/docs/search/latest/csharp?searchTerm=management-studio).
 * Go to the [query view](https://ravendb.net/docs/search/latest/csharp?searchTerm=query%20view).
 * From the indexes select `Subscriptions`
 * Enter the following query: `Subscribers:*"<endpointAddress>"*` and replace `<endpointAddress>` with the address of the distributor endpoint. Make sure to escape special characters like `.` or `-` with `\`. E.g. `my\.endpoint@distributor\-machine`.
 * Run the query.
 * On all listed documents, remove the entry with the distributor's subscriber address and save it.

See the following sample documents on how to remote the distributor subscription:

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


### Removing subscriptions from [NHibernate persistence](/persistence/nhibernate)

Remove the distributor from the publisher's subscription storages by removing all subscriptions related to the distributor's address. See [NHibernate Persistence Scripting](/persistence/nhibernate/scripting.md) on how to remove subscriptions.


### Removing subscriptions from [Azure Storage persistence](/persistence/azure-table)

Remove the distributor from the publisher's subscription storages by removing all subscriptions related to the distributor's address. See [Azure Storage Persistence Scripting](/persistence/azure-table/scripting.md) on how to remove subscriptions.


## Use of the distributor in mixed version environments

Environments consisting entirely of NServiceBus version 6 endpoints no longer require distributor endpoints. NServiceBus version 5 endpoints can communicate directly to NServiceBus version 6 endpoints but the sender-side distribution feature of version 6 is not compatible with endpoints using NServiceBus version 5.

 * NServiceBus version 6 endpoints must route their messages to a distributor when communicating with scaled out NServiceBus version 5 instances.
 * NServiceBus version 5 endpoints must route their messages to a distributor when communicating with scaled out NServiceBus version 6 instances. In this scenario, the NServiceBus version 6 instances must [enlist with a NServiceBus version 5 distributor](/transports/msmq/distributor/configuration.md#worker-configuration-when-self-hosting).
