---
title: MassTransit Connector for ServiceControl
summary: Introduction to the MassTransit Connector for ServiceControl
reviewed: 2024-09-04
component: ServiceControl
---

The [MassTransit Connector for ServiceControl](https://hub.docker.com/r/particular/servicecontrol-masstransit-connector) is part of the Particular Service Platform, which adds error queue and dead letter queue monitoring to MassTransit systems. This container runs alongside the existing MassTransit system and monitors for any faulted messages that occur within the system.

## How the MassTransit Connector works

The MassTransit Connector for ServiceControl container monitors for the existence of any error queues, or of the error queues specified in the `queues.txt` file if it exists. These error queues contain messages that have faulted and have been moved out of the consumer's input queue. The container then takes that message, ensures the correct format of all the headers, and moves the faulted message to the input queue of ServiceControl.

ServiceControl reads the faulted message, extracting information and metadata about the fault. It also indexes the metadata, allowing operations and support teams to see the faulted message within the ServicePulse dashboard.

## What queues are created

The ServiceControl MassTransit Connector creates a queue for transferring messages from ServiceControl back to a consumer's input queue. By default, the queue name is `Particular.ServiceControl.Connector.MassTransit_return`, which can be changed by overwriting the default value using the `RETURN_QUEUE` environment variable.

It also creates a "poison" queue that starts with the same name as the `RETURN_QUEUE` and appends `.poison` to it. By default, the queue name is `Particular.ServiceControl.Connector.MassTransit_return.poison`. This queue is  only used if there are any messages that the connector fails to process. Should any messages fail to this queue, a warning is raised in the `Custom Checks` section of ServicePulse

In addition, ServiceControl creates queues necessary to facilitate the process of retrying failed messages. These are:

* `error`
* `Particular.ServiceControl`
* `Particular.ServiceControl.staging`
* `Particular.ServiceControl.errors`

The `error` queue is where faulted messages are sent so that ServiceControl can ingest them.

The `Particular.ServiceControl` queue is used to handle requests triggered from the ServicePulse dashboard, such as retrying groups of messages.

The `Particular.ServiceControl.staging` queue ensures that messages aren't duplicated while retried. Messages sent back to consumers are first staged in this queue (as part of a single operation). Once all the messages are staged, they are forwarded to the original consumer.

The `Particular.ServiceControl.errors` queue tracks any internal errors that may occur within ServiceControl.

Other transport-specific queues might also be created. For example, when using RabbitMQ, a queue called `nsb.v2.verify-stream-flag-enabled` will be created to validate that the setup of the RabbitMQ broker enables streams and quorum queues.
