---
title: Transport configuration
reviewed: 2024-04-05
component: ServiceControl
---
ServiceControl can be configured to use one of the supported [transports](/transports/) listed below:

* [Azure Service Bus](/transports/azure-service-bus)
* [Azure Storage Queues](/transports/azure-storage-queues/)
* [Amazon Simple Queue Service (SQS)](/transports/sqs/)
* [Microsoft Message Queuing (MSMQ)](/transports/msmq/)
* [RabbitMQ - Conventional routing topology](/transports/rabbitmq/routing-topology.md#conventional-routing-topology)
* [RabbitMQ - Direct routing topology](/transports/rabbitmq/routing-topology.md#direct-routing-topology)
* [SQL Server](/transports/sql/)

## Transport-specific features

### Transport adapters

Certain transport features are not supported natively by ServiceControl and will require a [transport adapter](/servicecontrol/transport-adapter). Contact [support](https://particular.net/support) for further guidance.

Configuring third-party transports is not supported.

### MSMQ

To configure MSMQ as the transport, ensure the MSMQ service has been installed and configured as outlined in [MSMQ configuration](/transports/msmq/#msmq-configuration).

> [!IMPORTANT]
> When [ServiceControl instances are installed on a different machine than an endpoint](/transports/msmq/routing.md#when-servicecontrol-is-installed-on-a-different-server) `queuename@machinename` addresses must be used.

#### Configuration using PowerShell or Containers

When managing ServiceControl via PowerShell or when using containers, the transport must be specified using the `MSMQ` constant.

To do this in PowerShell set the `-Transport` option:

snippet: MSMQPowerShellTransport

To do this in Docker or other container hosts set the `TRANSPORTTYPE` environment variable:

snippet: MSMQDockerTransport

### RabbitMQ

In addition to the [connection string options of the transport](/transports/rabbitmq/connection-settings.md) the following ServiceControl specific options are available in versions 4.4 and above:

* `UseExternalAuthMechanism=true|false(default)` - Specifies that an [external authentication mechanism should be used for client authentication](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-external-authentication).
* `DisableRemoteCertificateValidation=true|false(default)` - Allows ServiceControl to connect to the broker [even if the remote server certificate is invalid](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-remote-certificate-validation).

#### Configuration using PowerShell or Containers

When managing ServiceControl via PowerShell or when using containers, the transport, queue type, and topology must be specified using the correct constant:

| Queue Type | Topology | Constant |
| --- | --- | --- |
| [Quorum](https://www.rabbitmq.com/docs/quorum-queues) | [Conventional](/transports/rabbitmq/routing-topology.md#conventional-routing-topology) | `RabbitMQ.QuorumConventionalRouting` |
| [Quorum](https://www.rabbitmq.com/docs/quorum-queues) | [Direct](/transports/rabbitmq/routing-topology.md#direct-routing-topology) | `RabbitMQ.QuorumDirectRouting` |
| [Classic](https://www.rabbitmq.com/docs/classic-queues) | [Conventional](/transports/rabbitmq/routing-topology.md#conventional-routing-topology) | `RabbitMQ.ClassicConventionalRouting` |
| [Classic](https://www.rabbitmq.com/docs/classic-queues) | [Direct](/transports/rabbitmq/routing-topology.md#direct-routing-topology) | `RabbitMQ.ClassicDirectRouting` |

To do this in PowerShell set the `-Transport` option:

snippet: RabbitMQPowerShellTransport

To do this in Docker or other container hosts set the `TRANSPORTTYPE` environment variable:

snippet: RabbitMQDockerTransport

### Azure ServiceBus

In addition to the [connection string options of the transport](/transports/azure-service-bus/configuration.md#configuring-an-endpoint) the following ServiceControl specific options are available in versions 4.4 and above:

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

#### Configuration using PowerShell or Containers

When managing ServiceControl via PowerShell or when using containers, the transport must be specified using the `NetStandardAzureServiceBus` constant.

To do this in PowerShell set the `-Transport` option:

snippet: AzureServiceBusPowerShellTransport

To do this in Docker or other container hosts set the `TRANSPORTTYPE` environment variable:

snippet: AzureServiceBusDockerTransport

### SQL

In addition to the [connection string options of the transport](/transports/sql/connection-settings.md#connection-configuration) the following ServiceControl specific options are available in versions 4.4 and above:

* `Queue Schema=<schema_name>` - Specifies custom schema for the ServiceControl input queue.
* `Subscriptions Table=<subscription_table_name>` - Specifies SQL subscription table name.
  * *Optional* `Subscriptions Table=<subscription_table_name>@<schema>` - to specify the schema.
  * *Optional* `Subscriptions Table=<subscription_table_name>@<schema>@<catalog>` - to specify the schema and catalog.

#### Configuration using PowerShell or Containers

When managing ServiceControl via PowerShell or when using containers, the transport must be specified using the `SQLServer` constant.

To do this in PowerShell set the `-Transport` option:

snippet: SQLServerPowerShellTransport

To do this in Docker or other container hosts set the `TRANSPORTTYPE` environment variable:

snippet: SQLServerDockerTransport

### Amazon SQS

The following ServiceControl connection string options are available:

* `AccessKeyId=<value>` - AssessKeyId value,
* `SecretAccessKey=<value>` - SecretAccessKey value,
* `Region=<value>` - Region transport [option](/transports/sqs/configuration-options.md#region),
* `QueueNamePrefix=<value>` - Queue name prefix transport [option](/transports/sqs/configuration-options.md#queue-name-prefix),
* `TopicNamePrefix=<value>` - Topic name prefix transport [option](/transports/sqs/configuration-options.md#topic-name-prefix)
* `S3BucketForLargeMessages=<value>` - S3 bucket for large messages [option](/transports/sqs/configuration-options.md#offload-large-messages-to-s3),
* `S3KeyPrefix=<value>` - S3 key prefix [option](/transports/sqs/configuration-options.md#offload-large-messages-to-s3-key-prefix).
* `DoNotWrapOutgoingMessages=true` - Do not wrap outgoing messages [option](/transports/sqs/configuration-options.md#do-not-wrap-message-payload-in-a-transport-envelope).

#### Configuration using PowerShell or Containers

When managing ServiceControl via PowerShell or when using containers, the transport must be specified using the `AmazonSQS` constant.

To do this in PowerShell set the `-Transport` option:

snippet: AmazonSQSPowerShellTransport

To do this in Docker or other container hosts set the `TRANSPORTTYPE` environment variable:

snippet: AmazonSQSDockerTransport