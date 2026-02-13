---
title: Transport configuration
summary: ServiceControl can be configured to use one of the supported message transports which are configured for each instance type
reviewed: 2025-10-22
component: ServiceControl
---

ServiceControl can be configured to use one of the supported [message transports](/transports/) which are configured for each instance type using the following settings:

* ServiceControl (Error) instance: [`ServiceControl/TransportType`](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicecontroltransporttype)
* Audit instance: [`ServiceControl.Audit/TransportType`](/servicecontrol/audit-instances/configuration.md#transport-servicecontrol-audittransporttype)
* Monitoring instance: [`Monitoring/TransportType`](/servicecontrol/monitoring-instances/configuration.md#transport-monitoringtransporttype)

The value for the `TransportType` settings can be any of the following:

| Message Transport | `TransportType` values |
|-|-|
| [Azure Service Bus](#azure-service-bus) | `NetStandardAzureServiceBus` |
| [Azure Storage Queues](#azure-storage-queues) | `AzureStorageQueue` |
| [Amazon Simple Queue Service (SQS)](#amazon-sqs) | `AmazonSQS` |
| [RabbitMQ](#rabbitmq)<br/><i>See topology options below.</i> | `RabbitMQ.QuorumConventionalRouting`<br/>`RabbitMQ.ClassicConventionalRouting`<br/>`RabbitMQ.QuorumDirectRouting`<br/>`RabbitMQ.ClassicDirectRouting` |
| [SQL Server](#sql) | `SQLServer` |
| [PostgreSQL](#postgresql) | `PostgreSQL` |
| [Microsoft Message Queuing (MSMQ)](#msmq) | `MSMQ` |


## Azure Service Bus

### Topic-per-event topology for integration events

Starting with version 6.4.0, ServiceControl runs versions of the Azure Service Bus transport that, by default, use [topic-per-event topology](/transports/azure-service-bus/topology.md), as opposed to previously used [single-topic topology](/transports/azure-service-bus/topology.md?version=asbs_4). This breaking change affects the publishing of [integration events](/servicecontrol/contracts.md).

In order to continue using the single-topic topology, the topic name has to be specified explicitly using the `TopicName=<topic-bundle-name>` connection string option.

> [!WARNING]
> If any subscribers exist for these integration events and these endpoints have not yet been upgraded to NServiceBus.Transport.AzureServiceBus 5.x, then ServiceControl must be configured to use `TopicName=bundle-<topic-bundle-name>`

The new topology uses the event type's full name as the name of the topic to which an event is published e.g. `servicecontrol.contracts.messagefailed`. This mapping can be customized by providing the [topology description in JSON](/transports/azure-service-bus/configuration.md#entity-creation-topology-mapping-options) using `ServiceControl.Transport.ASBS/Topology` application setting or `ServiceControl_Transport_ASBS_Topology` environment variable.

Furthermore, in addition to the [connection string options of the transport](/transports/azure-service-bus/configuration.md#configuring-an-endpoint) the following ServiceControl specific options are available in versions 4.4 and above:

* `TransportType=AmqpWebSockets` - Configures the transport to use [AMQP over websockets](/transports/azure-service-bus/configuration.md#connectivity).
* `TopicName=<topic-bundle-name>` - Specifies the [topic name](/transports/azure-service-bus/configuration.md#entity-creation) to be used by the instance. The default value is `bundle-1`.
* `QueueLengthQueryDelayInterval=<value_in_milliseconds>` - Specifies the delay between queue length refresh queries for queue length monitoring. The default value is 500 ms.

### Enabling Managed Identity

As of version 4.21.8 of ServiceControl, the following options can be used to enable [Managed Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview) authentication:

* Setting the connection string to a [fully-qualified namespace](https://docs.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusclient.fullyqualifiednamespace) (eg. `my-namespace.servicebus.windows.net`)
  * With this setting, a [`DefaultAzureCredential`](https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential) will be used.
  * No connection string options can be used when using a fully-qualified namespace.
* Specifying the connection string option `Authentication=Managed Identity`
  * The fully-qualified namespace will be parsed from the `Endpoint=sb://my-namespace.servicebus.windows.net/` connection string option
  * When specifying managed identity for the connection string, a [`ManagedIdentityCredential`](https://docs.microsoft.com/en-us/dotnet/api/azure.identity.managedidentitycredential) will be used.
  * Set the `ClientId=some-client-id` connectionstring option to use a specific [user-assigned identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview#managed-identity-types)

### Enabling Partitioned Entities

As of versions 4.33.3 and 5.0.5 of ServiceControl, support for partitioned entities can be enabled by adding the following connection string parameter:

* `EnablePartitioning=<True|False>` — Configures the transport to create entities that support partitioning. The default value is `false`.

### Enabling Hierarchical Entities

When [a hierarchy namespace prefix](/transports/azure-service-bus/configuration.md#entity-creation-hierarchy-namespace) is configured on endpoints, ServiceControl needs to be configured for the hierarchy namespace to monitor those endpoints.

At a minimum, the error queue name needs the prefix prepended to the queue name, separated by a `/`.  If monitoring or auditing are enabled, they also need to be modified.

For example, given a hierarchy namespace of `my-hierarchy` and error, audit, and monitoring queues named `error`, `audit`, and `monitoring`:
- The [error queue name](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicebuserrorqueue) would need to be set to `my-hierarchy/error`.
- The [audit queue name](/servicecontrol/audit-instances/configuration.md#transport-servicebusauditqueue) would need to be set to `my-hierarchy/audit`.
- The [monitoring queue name](/servicecontrol/monitoring-instances/configuration.md#transport-monitoringerrorqueue) would need to be set to `my-hierarchy/monitoring`.

This isolates the ServiceControl instance to the hierarchy, so an instance-per-hierarchy would be required to monitor multiple hierarchies.

#### Configuring a single ServiceControl instance for multiple hierarchies

Endpoints configured with a hierarchy namespace will use error, audit, and monitoring queues prefixed with the hierarchy namespace.  To monitor all hierarchies with a single ServiceControl installation:
1. Configure ServiceControl error, audit, and monitoring instances as you would without using a hierarchy namespace.
2. For each hierarchy-specific error, audit, and monitoring queue that you wish to monitor centrally, configure [auto-forwarding for their corresponding queues in azure](https://learn.microsoft.com/en-us/azure/service-bus-messaging/enable-auto-forward#update-the-auto-forward-setting-for-an-existing-queue) to forward messages to the central error, audit, and monitoring queues.

### Example connection string

```text
Endpoint=sb://[namespace].servicebus.windows.net; SharedSecretIssuer=<owner>;SharedSecretValue=<someSecret>;QueueLengthQueryDelayInterval=<IntervalInMilliseconds(Default=500ms)>;TopicName=<TopicBundleName(Default=bundle-1)>;EnablePartitioning=<true|false(Default=false)>
```

## Azure Storage Queues

ServiceControl does not add any connection settings beyond the Azure Storage connection string.

### Example connection string

```text
DefaultEndpointsProtocol=[http|https];AccountName=<MyAccountName>;AccountKey=<MyAccountKey>;Subscriptions Table=tablename
```

## RabbitMQ

RabbitMQ contains different `TransportType` options based on the topology and queue type used by the system:

| Queue Type | Topology | `TransportType` value |
| --- | --- | --- |
| [Quorum](https://www.rabbitmq.com/docs/quorum-queues) | [Conventional](/transports/rabbitmq/routing-topology.md#conventional-routing-topology) | `RabbitMQ.QuorumConventionalRouting` (Preferred) |
| [Quorum](https://www.rabbitmq.com/docs/quorum-queues) | [Direct](/transports/rabbitmq/routing-topology.md#direct-routing-topology) | `RabbitMQ.QuorumDirectRouting` |
| [Classic](https://www.rabbitmq.com/docs/classic-queues) | [Conventional](/transports/rabbitmq/routing-topology.md#conventional-routing-topology) | `RabbitMQ.ClassicConventionalRouting` |
| [Classic](https://www.rabbitmq.com/docs/classic-queues) | [Direct](/transports/rabbitmq/routing-topology.md#direct-routing-topology) | `RabbitMQ.ClassicDirectRouting` |

In addition to the [connection string options of the transport](/transports/rabbitmq/connection-settings.md), the following options are available in versions 4.4 and above:

* `UseExternalAuthMechanism=true|false(default)` - Specifies that an [external authentication mechanism should be used for client authentication](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-external-authentication).
* `DisableRemoteCertificateValidation=true|false(default)` - Allows ServiceControl to connect to the broker [even if the remote server certificate is invalid](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-remote-certificate-validation).

These options are available in version 6.5 and above:

* `ManagementApiUrl=<SCHEME://HOST:PORT>` - The URL of the RabbitMQ management API. If this option is not set, the transport will attempt to automatically generate the URL based on the broker connection string.
* `ManagementApiUserName=<USERNAME>` - The username used to connect to the RabbitMQ management API. If this option is not set, the credentials from the broker connection string will be used.
* `ManagementApiPassword=<PASSWORD>` - The password used to connect to the RabbitMQ management API. If this option is not set, the credentials from the broker connection string will be used.
* `ValidateDeliveryLimits=<true(default)|false>` - Controls the [delivery limit validation](/transports/rabbitmq/connection-settings.md#delivery-limit-validation) of the ServiceControl queues.

### Example connection string

```text
host=<HOSTNAME>;username=<USERNAME>;password=<PASSWORD>;DisableRemoteCertificateValidation=<true|false(default)>;UseExternalAuthMechanism=<true|false(default)>;ValidateDeliveryLimits=<true(default)|false>;ManagementApiUrl=<SCHEME://HOST:PORT>;ManagementApiUserName=<USERNAME>;ManagementApiPassword=<PASSWORD>
```

## SQL

In addition to the [connection string options of the transport](/transports/sql/connection-settings.md#connection-configuration) the following ServiceControl specific options are available in versions 4.4 and above:

* `Queue Schema=<schema_name>` - Specifies custom schema for the ServiceControl input queue.
* `Subscriptions Table=<subscription_table_name>` - Specifies SQL subscription table name.
  * *Optional* `Subscriptions Table=<subscription_table_name>@<schema>` - to specify the schema.
  * *Optional* `Subscriptions Table=<subscription_table_name>@<schema>@<catalog>` - to specify the schema and catalog.

### Example connection string

```text
Data Source=<SQLInstance>;Initial Catalog=nservicebus;Integrated Security=True;Queue Schema=myschema;Subscriptions Table=tablename@schema@catalog
```

## PostgreSQL

In addition to the [connection string options of the transport](/transports/postgresql/connection-settings.md#connection-configuration) the following ServiceControl specific options are available in versions 5.10 and above:

* `Queue Schema=<schema_name>` - Specifies a custom schema for the ServiceControl input queue.
* `Subscriptions Table=<subscription_table_name>` - Specifies PostgreSQL subscription table name.
  * *Optional* `Subscriptions Table=schema.tablename` - to specify the schema with simple table name.
  * *Optional* `Subscriptions Table=schema.multi.table.name` - to specify the schema with a table name containing `.`.
  * *Optional* `Subscriptions Table==&quot;multi.table.name=&quot;` - to specify a table name containing `.` without a schema. In this case, `Queue Schema` will be used if specified, otherwise the default schema (`public`) will be used.

### Example connection string

```text
Server=<ServerName>;Database=nservicebus;Port=5432;User Id=<Username>;Password=<Password>;Queue Schema=myschema;Subscriptions Table=schema.tablename
```

## Amazon SQS

The following ServiceControl connection string options are available:

* `AccessKeyId=<value>` - AssessKeyId value,
* `SecretAccessKey=<value>` - SecretAccessKey value,
* `Region=<value>` - Region transport [option](/transports/sqs/configuration-options.md#region),
* `QueueNamePrefix=<value>` - Queue name prefix transport [option](/transports/sqs/configuration-options.md#queue-name-prefix),
* `TopicNamePrefix=<value>` - Topic name prefix transport [option](/transports/sqs/configuration-options.md#topic-name-prefix)
* `S3BucketForLargeMessages=<value>` - S3 bucket for large messages [option](/transports/sqs/configuration-options.md#offload-large-messages-to-s3),
* `S3KeyPrefix=<value>` - S3 key prefix [option](/transports/sqs/configuration-options.md#offload-large-messages-to-s3-key-prefix).
* `DoNotWrapOutgoingMessages=true` - Do not wrap outgoing messages [option](/transports/sqs/configuration-options.md#do-not-wrap-message-payload-in-a-transport-envelope).
* `ReservedBytesInMessageSize=<value>` - Available from 7.3 - Reserve bytes in message size calculation [option](/transports/sqs/configuration-options.md#reserve-bytes-when-calculating-message-size).

> [!NOTE]
> When using SQS as a transport, for local development purposes it is possible to set up ServiceControl to connect to a LocalStack instance.
> Refer to the [documentation](/nservicebus/aws/local-development.md) about how to configure the environment to use LocalStack.

### Example connection string

```text
Region=<REGION>;QueueNamePrefix=<prefix>;TopicNamePrefix=<prefix>;AccessKeyId=<ACCESSKEYID>;SecretAccessKey=<SECRETACCESSKEY>;S3BucketForLargeMessages=<BUCKETNAME>;S3KeyPrefix=<KEYPREFIX>
```

## MSMQ

To configure MSMQ as the transport, ensure the MSMQ service has been installed and configured as outlined in [MSMQ configuration](/transports/msmq/#msmq-configuration).

> [!NOTE]
> Any settings that specify a queue name for a queue that is not located on the same machine as the ServiceControl instance must use `queuename@machinename` addresses to refer to that queue.

> [!WARNING]
> MSMQ transport is not available when running ServiceControl on containers.
