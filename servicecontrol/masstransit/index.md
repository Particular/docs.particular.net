---
title: ServiceControl MassTransit Connector
summary: Configuration and running the Particular Platform MassTransit connector
reviewed: 2024-09-04
component: ServiceControl
---

## ServiceControl MassTransit Connector

The [ServiceControl MassTransit Connector](https://hub.docker.com/r/particular/servicecontrol.connector.masstransit) is part of the Particular Service Platform which adds error queue and dead letter queue monitoring to MassTransit systems. This container runs alongside the existing MassTransit system and monitors for any faulted messages that occur within the system.

## How the MassTransit Connector works

The connector container monitors for the existence of any `_error` queues in the broker. If there are any `_error` queues, it means that a message has faulted and has been moved out of the consumer's input queue. The container then takes that message, ensures correct format of all the headers, and moves the faulted message to the input queue of ServiceControl.

ServiceControl reads the faulted message, extracting information and metadata about the faulted message. It also indexes the metadata allowing operations and support teams to see the faulted mesasge within the ServicePulse dashboard.

## What queues are created

The ServiceControl MassTransit Connector creates a queue used for transferring messages from ServiceControl back to the input queue of a consumer. By default, the queue name is `Particular.ServiceControl.Connector.MassTransit_return` which can be changed by overrading the default value using the `RETURNQUEUE` environment variable.

In addition, ServiceControl creates queues necessary to facilitate the process of retrying failed messages. These are:

* `error`
* `Particular.ServiceControl`
* `Particular.ServiceControl.staging`
* `Particular.ServiceControl.errors`

The `error` queue is where faulted messages are sent so that they can be ingested by ServiceControl.

The `Particular.ServiceControl` queue is used to handle requests tiggered from the ServicePulse dashboard e.g. retrying groups of messages.

The `Particular.ServiceControl.staging` queue is used to ensure that messages aren't duplicated while retried. Messages sent back to consumers are first staged in this queue (as part of a single operation). Once all the messages are staged only than are they forwarded to the original consumer.

The `Particular.ServiceControl.errors` queue is used to track any internal errors that may occur within ServiceControl.

Other, transport specific queues, might also get created. For example when using RabbitMQ, a queue called `nsb.v2.verify-stream-flag-enabled` will be created to validate that the setup of the RabbitMQ broker enables streams and quorum queues.

## Settings

The connector container can be configured using set of options provided via environment variables.


| Key              | Description                                                                                         | Default                                                  |
|------------------|-----------------------------------------------------------------------------------------------------|----------------------------------------------------------|
| TRANSPORTTYPE    | The transport type                                                                                  | None                                                     |
| CONNECTIONSTRING | The NServiceBus connection string for the specified transport                                       | None                                                     |
| RETURNQUEUE      | The queue used by the connector to transfer  messages between ServiceControl and MassTransit consumer input queue | `Particular.ServiceControl.Connector.MassTransit_return` |
| ERRORQUEUE       | ServiceControl error queue                                                              | `error`                                                  |
| MANAGEMENTAPI    | RabbitMQ management API URL (neede only when using RabbitMQ)                                  | None                                                     |
| QUEUES_FILE      | Path to a file with an explicit list of error queue names (each name in a separate line) for the connector to monitor                                   | None                                                     |
| QUEUENAMEPREFIX  | Only monitor queues that have this prefix. E.g. `Dev_` | None |
| QUEUENAMESUFFIX  | Only monitor queues that have this suffix. E.g. `_Dev` | None |
| QUEUENAMEFILTER  | Only monitor queues that contain this string. E.g. `Dev` | None |

### TRANSPORTTYPE

Currently support as the most used MassTransit transports: Amazon SQS, Azure Service Bus and RabbitMQ.

| Description       | Key                                  |
|-------------------|--------------------------------------|
| Amazon SQS        | `AmazonSQS`                          |
| Azure Service Bus | `NetStandardAzureServiceBus`         |
| RabbitMQ          | `RabbitMQ.QuorumConventionalRouting` |

> [!NOTE]
> The user account used to connect to RabbitMQ requires [Management API](#settings-managementapi) access in order to function.

### CONNECTIONSTRING

The connection string format used is the same for all ServiceControl components.

- [Azure Service Bus connection string format](/servicecontrol/transports.md#azure-service-bus)
- [RabbitMQ connection string format](/servicecontrol/transports.md#rabbitmq)
- [AmazonSQS connection string format](/servicecontrol/transports.md#amazon-sqs)

### RETURNQUEUE

Default: `Particular.ServiceControl.Connector.MassTransit_return`

The queue used by the connector to transfer  messages between ServiceControl and MassTransit consumer input queue

### ERRORQUEUE

Default: `error`

ServiceControl by default listens to the `error` queue. When this value is overriden in ServiceControl this configuration setting must be set to the same value.

### MANAGEMENTAPI

Default: None

> [!NOTE]
>  Applies to RabbitMQ only

Required when using RabbitMQ and using dynamic discovery of MassTransit error queues. The url needs to contain the username and password used to authenticate.

Example:

```txt
http://guest:guest@localhost:15672
```

### QUEUES_FILE

Default: None

Path that contains a static list of queue names to monitor. If no value is specified the connector will run auto-discovery mode.

Example:

```txt
/queues.txt
```

### Queue Name Filtering

Specifying a value for QUEUENAMEREGEXFILTER allows the list of queues that are monitored to be filtered by the provided Regular Expression. This can be useful in scenarios where only part of a system must have the failed messages managed by the Particular Platform.

Example:

```txt
// Only monitor queues that start with Dev
QUEUENAMEREGEXFILTER=^Dev

// Only monitor queues the end with Dev
QUEUENAMEREGEXFILTER=Dev$

// Only monitor queues that contain Dev
QUEUENAMEREGEXFILTER=Dev
```

If the broker had the following queues:
```txt
SalesReport
SalesDevReport
Orders
OrdersReport
OrdersDev
```

When specifying `QUEUENAMEREGEXFILTER=Dev`, then the following queues would be monitored for failures:

```txt
SalesDevReport
OrdersDev
```