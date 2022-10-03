---
title: Migrating RabbitMQ from classic to quorum queues
summary: Instructions on how to migrate from classic queues to quorum queues
reviewed: 2022-09-29
component: Rabbit
---

[RabbitMQ quorum queues](https://www.rabbitmq.com/quorum-queues.html) are superior to classic queues for use cases where data safety is a top priority, and are recommended for all NServiceBus endpoints. In most cases, migration to quorum queues can be accomplished on a live system, without downtime for the whole system.

WARN: Systems using the direct routing topology cannot be updated without system downtime. Contact [Particular support](https://particular.net/support) to discuss options for updating to quorum queues.

## Migration steps

The following steps can be followed to migrate queues from classic queues to quorum queues. Each step is self-contained, so the system is usable after each step in the process.

### Step 1: Upgrade broker

The broker must be running version 3.10.0 or higher. The `stream_queue` and `quorum_queue` [feature flags](https://www.rabbitmq.com/feature-flags.html) also must be enabled.

The broker requirements can be verified with the [delays verify](/transports/rabbitmq/operations-scripting.md#delays-verify) command provided by the command line tool.

### Step 2: Verify current transport version

Before starting the migration progress, all endpoints should be using version 6.1.1 of the RabbitMQ transport. This specific version allows endpoints which have not yet been migrated to quorum queues to continue to work whether or not the error and audit queues have been migrated.

{{WARN:
Do not update endpoints to version 7.0.0 or higher of the transport at this time. Version 6.1.1 will allow error and audit queues to be either classic or quorum queues, which allows updating one endpoint at a time.

If endpoints have already been updated to version 7.0.0 while using classic queues, [disable installers](/nservicebus/operations/installers.md) on all endpoints before continuing the migration to prevent endpoints from failing to start after Step 4.
}}

### Step 3: Upgrade ServiceControl and update instances

Starting in ServiceControl version 4.23.0, there are now four RabbitMQ transport types:

* RabbitMQ - Conventional routing topology (classic queues)
* RabbitMQ - Conventional routing topology (quorum queues)
* RabbitMQ - Direct routing topology (classic queues)
* RabbitMQ - Direct routing topology (quorum queues)

Updating an instance using the conventional routing topology will automatically update the ServiceControl transport to "RabbitMQ - Conventional routing topology (classic queues)".

To get started, download the latest version of ServiceControl from the [downloads page](https://particular.net/downloads) and install it. Then, update each instance to the latest version.

### Step 4: Migrate audit and error queues

Use the [`queue migrate-to-quorum`](/transports/rabbitmq/operations-scripting.md#queue-migrate-to-quorum) command provided by the command line tool to migrate all audit and error queues to quorum queues.

### Step 5: Migrate ServiceControl queues

Use the [`queue migrate-to-quorum`](/transports/rabbitmq/operations-scripting.md#queue-migrate-to-quorum) command provided by the command line tool to migrate all ServiceControl queues (such as `Particular.ServiceControl`) to quorum queues.

### Step 6: Change the transport of the ServiceControl instances

Now that the error, audit, and ServiceControl queues have been migrated, the transport of each ServiceControl instance must be modified to use quorum queues, which can only be done by manually editing the configuration file.

To get started, open the ServiceControl Management Utility.

For each deployed ServiceControl instance:

1. Under **Installation Path**, click **Browse**.
1. In the Explorer window that appears, locate and edit the **ServiceControl.exe.config** file.
1. Change the value of the `ServiceControl/TransportType` setting to `ServiceControl.Transports.RabbitMQ.RabbitMQQuorumConventionalRoutingTransportCustomization, ServiceControl.Transports.RabbitMQ`.
1. In ServiceControl Management Utility, start the instance.

### Step 7: Migrate endpoints

Endpoints can now be migrated to use quorum queues as their input queues on a per-endpoint basis.

For each endpoint:

1. Update to NServiceBus.RabbitMQ 7.0.0.
1. In the endpoint configuration code, set the [queue type](/transports/rabbitmq/routing-topology.md#controlling-queue-type) to use quorum queues.
1. Use the [`queue migrate-to-quorum`](/transports/rabbitmq/operations-scripting.md#queue-migrate-to-quorum) command provided by the command line tool to migrate all queues used by the endpoint to quorum queues. In addition to the queue with the same name as the endpoint, there may be queues named according to the pattern `{EndpointName}-{Discriminator}` which also need to be migrated.
1. Deploy the new version of the endpoint code.

INFO: If installers were previously disabled because endpoints were already updated to version 7.0.0, the installers can be re-enabled as part of the per-endpoint process here.

## Step 8: Migrate delayed messages

Starting with NServiceBus.RabbitMQ version 7.0.0, there is a new version of the delay infrastructure based on quorum queues, which ensures that delayed messages cannot be lost in the event of a network partition.

* The "v1" infrastructure contains 27 queues with the pattern `nsb.delay-level-##`, which use classic queues
* The "v2" infrastructure contains 27 queues with the pattern `nsb.v2.delay-level-##` which use quorum queues.

The two delay infrastructures can be used in parallel for as long as necessary, but delayed messages migrated to the v2 delay infrastructure are safer than in the v1 infrastructure.

Use the [`delays migrate`](/transports/rabbitmq/operations-scripting.md#delays-migrate) command provided by the command line tool to move all existing delayed messages to the v2 delay infrastructure. This command can be run safely multiple times until all endpoints are updated to version 7.0.0, and the tool confirms that all delayed messages have been moved to the new infrastructure.

Delayed message migration can be done after each endpoint is upgraded, or only once at the end of the process, after every endpoint is updated to version 7.0.0. Messages that are migrated are safe from network partitions; however, as long as there are still endpoints that have not been updated to version 7.0.0, it's possible for additional messages to be delivered to the v1 delay infrastructure. This is why the tool must be run at the very least after all endpoints are migrated.

## Step 9: Remove v1 delay infrastructure

After all delayed messages have been migrated, the exchanges and queues that make up the v1 delay infrastructure (queues and exchanges matching `nsb.delay-level-##` and the `nsb.delay-delivery` exchange) can be removed from the broker.
