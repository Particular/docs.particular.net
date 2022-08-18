---
title: Migrating from classic queues to quorum queues
summary: Instructions on how to migrate from classic queues to quorum queues
reviewed: 2022-05-05
component: Rabbit
---

## Migration steps

The following steps can be followed to migrate queues from classic to quorum. Each step is self-contained, so the system is usable at every point in the process.

### Step 1: Upgrade broker

The broker needs to be running 3.10.0 or higher. The `stream_queue` and `quorum_queue` [feature flags](https://www.rabbitmq.com/feature-flags.html) also need to be enabled.

The broker requirements can be verified with the [delays verify](/transports/rabbitmq/operations-scripting.md#delays-verify) command provided by the command line tool.

### Step 2: Verify current transport version

Before starting the migration progress, all endpoints should be using v6.1.1 of the transport.

### Step 3: Verify current ServiceControl version

Before starting the migration progress, ensure ServiceControl is upgraded to 4.21.8.

### Step 4: Migrate audit and error queues
 
 Use the [`queue migrate-to-quorum`](/transports/rabbitmq/operations-scripting.md#queue-migrate-to-quorum) command provided by the command line tool to migrate all audit and error queues to quorum queues.

### Step 5: Migrate ServiceControl queues

 Use the [`queue migrate-to-quorum`](/transports/rabbitmq/operations-scripting.md#queue-migrate-to-quorum) command provided by the command line tool to migrate all ServiceControl queues to quorum queues.

### Step 6: Upgrade ServiceControl to 4.23.0 or later

 Install the latest version of ServiceControl and upgrade the instances. For each instance, before starting it, edit the instance's `ServiceControl.exe.config` file to change the value of the `ServiceControl/TransportType` setting to `ServiceControl.Transports.RabbitMQ.RabbitMQQuorumConventionalRoutingTransportCustomization, ServiceControl.Transports.RabbitMQ`.

### Step 7: Update endpoints to transport v7.0.0 or later

As part of this upgrade, set the [queue type](/transports/rabbitmq/routing-topology.md#controlling-queue-type) to use classic queues.

## Step 8: Migrate delayed messages from v1 delay infrastructure to v2

Once all endpoints are upgraded to 7.0.0 or later, use the [`delays migrate`](/transports/rabbitmq/operations-scripting.md#delays-migrate) to move all existing delayed messages to the v2 delay infrastructure.

## Step 9: Remove v1 delay infrastructure

After all delayed messages have been migrated, the exchanges and queues that make up the v1 delay infrastructure can be removed from the broker.

### Step 10: Migrate endpoints

Endpoints can now be migrated on a per-endpoint basis:

1. Shut down the endpoint
1. Use the [`queue migrate-to-quorum`](/transports/rabbitmq/operations-scripting.md#queue-migrate-to-quorum) command to migrate all of the endpoint's queues to quorum queues
1. Deploy a new version of the endpoint that sets the [queue type](/transports/rabbitmq/routing-topology.md#controlling-queue-type) to quorum queues.
1. Restart the endpoint.