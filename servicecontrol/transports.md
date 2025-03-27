---
title: Transport configuration
summary: ServiceControl can be configured to use one of the supported message transports which are configured for each instance type
reviewed: 2024-07-19
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

Follow the link for each transport for additional information on configuration options for that transport lower on this page.

## Azure Service Bus

Starting from version 6.4, ServiceControl runs Azure Service Bus transport that, by default, uses [topic-per-event topology](/transports/azure-service-bus/topology.md), as opposed to previously used [single-topic topology](/transports/azure-service-bus/topology.md?version=asbs_4). This change affects the publishing of [integration events](/servicecontrol/contracts.md). In oder to continue using the single-topic topology, the topic name has to be specified exlicitly using the `TopicName=<topic-bundle-name>` connection string option.

The new topology uses event type's full name as the name of the topic to which an event is published e.g. `servicecontrol.contracts.messagefailed`. This mapping can be customized by providing the [topology description in json](/transports/azure-service-bus/configuration.md#entity-creation-topology-mapping-options) using `ServiceControl.Transport.ASBS/Toplogy` application setting or `ServiceControl_Transport_ASBS_Toplogy` environment variable.

Furthermore, in addition to the [connection string options of the transport](/transports/azure-service-bus/configuration.md#configuring-an-endpoint) the following ServiceControl specific options are available in versions 4.4 and above:

* `TransportType=AmqpWebSockets` - Configures the transport to use [AMQP over websockets](/transports/azure-service-bus/configuration.md#connectivity).
* `TopicName=<topic-bundle-name>` - Specifies the [topic name](/transports/azure-service-bus/configuration.md#entity-creation) to be used by the instance. The default value is `bundle-1`.
* `QueueLengthQueryDelayInterval=<value_in_milliseconds>` - Specifies the delay between queue length refresh queries for queue length monitoring. The default value is 500 ms.

As of version 4.21.8 of ServiceControl, the following options can be used to enable [Managed Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview) authentication:

* Setting the connection string to a [fully-qualified namespace](https://docs.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusclient.fullyqualifiednamespace) (eg. `my-namespace.servicebus.windows.net`)
  * With this setting, a [`DefaultAzureCredential`](https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential) will be used.
  * No connection string options can be used when using a fully-qualified namespace.
* Specifying the connection string option `Authentication=Managed Identity`
  * The fully-qualified namespace will be parsed from the `Endpoint=sb://my-namespace.servicebus.windows.net/` connection string option
  * When specifying managed identity for the connection string, a [`ManagedIdentityCredential`](https://docs.microsoft.com/en-us/dotnet/api/azure.identity.managedidentitycredential) will be used.
  * Set the `ClientId=some-client-id` connectionstring option to use a specific [user-assigned identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview#managed-identity-types)

As of versions 4.33.3 and 5.0.5 of ServiceControl, support for partitioned entities can be enabled by adding the following connection string parameter:

* `EnablePartitioning=<True|False>` — Configures the transport to create entities that support partitioning. The default value is `false`.

## Azure Storage Queues

ServiceControl does not add any connection settings beyond the Azure Storage connection string.

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

## SQL

In addition to the [connection string options of the transport](/transports/sql/connection-settings.md#connection-configuration) the following ServiceControl specific options are available in versions 4.4 and above:

* `Queue Schema=<schema_name>` - Specifies custom schema for the ServiceControl input queue.
* `Subscriptions Table=<subscription_table_name>` - Specifies SQL subscription table name.
  * *Optional* `Subscriptions Table=<subscription_table_name>@<schema>` - to specify the schema.
  * *Optional* `Subscriptions Table=<subscription_table_name>@<schema>@<catalog>` - to specify the schema and catalog.

## PostgreSQL

In addition to the [connection string options of the transport](/transports/postgresql/connection-settings.md#connection-configuration) the following ServiceControl specific options are available in versions 5.10 and above:

* `Queue Schema=<schema_name>` - Specifies a custom schema for the ServiceControl input queue.
* `Subscriptions Table=<subscription_table_name>` - Specifies PostgreSQL subscription table name.
  * *Optional* `Subscriptions Table=schema.tablename` - to specify the schema with simple table name.
  * *Optional* `Subscriptions Table=schema.multi.table.name` - to specify the schema with a table name containing `.`.
  * *Optional* `Subscriptions Table==&quot;multi.table.name=&quot;` - to specify a table name containing `.` without a schema. In this case, `Queue Schema` will be used if specified, otherwise the default schema (`public`) will be used.

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

> [!NOTE]
> When using SQS as a transport, for local development purposes it is possible to set up ServiceControl to connect to a LocalStack instance.
> Refer to the [documentation](/nservicebus/aws/local-development.md) about how to configure the environment to use LocalStack.

## MSMQ

To configure MSMQ as the transport, ensure the MSMQ service has been installed and configured as outlined in [MSMQ configuration](/transports/msmq/#msmq-configuration).

> [!IMPORTANT]
> When [ServiceControl instances are installed on a different machine than an endpoint](/transports/msmq/routing.md#when-servicecontrol-is-installed-on-a-different-server) `queuename@machinename` addresses must be used.

> [!NOTE]
> MSMQ transport is not available when running ServiceControl on containers.
