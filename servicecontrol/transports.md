---
title: Transport support
reviewed: 2021-09-01
---
ServiceControl can be configured to use one of the supported [transports](/transports/) listed below using the ServiceControl Management application:

* [Azure Service Bus](/transports/azure-service-bus)
* [Azure Service Bus - Endpoint-oriented topology](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-endpoint-oriented-topology)
* [Azure Service Bus - Forwarding topology](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-forwarding-topology)
* [Azure Storage Queues](/transports/azure-storage-queues/)
* [Amazon Simple Queue Service (SQS)](/transports/sqs/)
* [Microsoft Message Queuing (MSMQ)](/transports/msmq/)
* [RabbitMQ - Conventional routing topology](/transports/rabbitmq/routing-topology.md#conventional-routing-topology)
* [RabbitMQ - Direct routing topology](/transports/rabbitmq/routing-topology.md#direct-routing-topology)
* [SQL Server](/transports/sql/)

### Transport-specific features

#### Transport adapters

Certain transport features are not supported natively by ServiceControl and will require a [transport adapter](/servicecontrol/transport-adapter). Contact support@particular.net for further guidance.

Configuring third-party transports through the ServiceControl Management application is not supported.

#### MSMQ

To configure MSMQ as the transport, ensure the MSMQ service has been installed and configured as outlined in [Installing The Platform Components](/platform/installer/offline.md#msmq-prerequisites).

#### RabbitMQ

In addition to the [connection string options of the transport](/transports/rabbitmq/connection-settings.md) the following ServiceControl specific options are available in versions 4.4 and above:

* `UseExternalAuthMechanism=true|false(default)` - Specifies that an [external authentication mechanism should be used for client authentication](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-external-authentication).
* `DisableRemoteCertificateValidation=true|false(default)` - Allows ServiceControl to connect to the broker [even if the remote server certificate is invalid](/transports/rabbitmq/connection-settings.md#transport-layer-security-support-remote-certificate-validation).

#### Azure Service Bus

In addition to the [connection string options of the transport](/transports/azure-service-bus/#configuring-an-endpoint) the following ServiceControl specific options are available in versions 4.4 and above:

* `TransportType=AmqpWebSockets` - Configures the transport to use [AMQP over websockets](/transports/azure-service-bus/configuration.md#connectivity).
* `TopicName=<topic-bundle-name>` - Specifies [topic name](/transports/azure-service-bus/configuration.md#entity-creation) to be used by the instance. The default value is `bundle-1`.
* `QueueLengthQueryDelayInterval=<value_in_milliseconds>` - Specifies delay between queue length refresh queries for the queue length monitoring. The default value is 500 ms.

As of Version 4.21.8 the following options can be used to enable [Mangaged Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview) authentication:

* Setting the connection string to a [fully qualified namespace](https://docs.microsoft.com/en-us/dotnet/api/azure.messaging.servicebus.servicebusclient.fullyqualifiednamespace) (eg. `my-namespace.servicebus.windows.net`)
  * In this mode a [`DefaultAzureCredential`](https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential) will be used.
  * No connection string options can be used in the mode.
* Specifying the connection string option `Authentication=Managed Identity`
  * The fully qualified namespace will be parsed from the `Endpoint=sb://my-namespace.servicebus.windows.net/` connection string option
  * In this mode a [`ManagedIdentityCredential`](https://docs.microsoft.com/en-us/dotnet/api/azure.identity.managedidentitycredential) will be used.
  * Set the  `ClientId=some-client-id` connectionstring option to use a specific [user assigned identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview#managed-identity-types) 

#### SQL

In addition to the [connection string options of the transport](/transports/sql/connection-settings.md#connection-configuration) the following ServiceControl specific options are available in versions 4.4 and above:

* `Queue Schema=<schema_name>` - Specifies custom schema for the ServiceControl input queue.
* `SubscriptionRouting=<subscription_table_name>` - Specifies SQL subscription table name.  

#### Amazon SQS

The following ServiceControl connection string options are available in versions 4.4 and above:

* `AccessKeyId=<value>` - AssessKeyId value,
* `SecretAccessKey=<value>` - SecretAccessKey value,
* `Region=<value>` - Region transport [option](/transports/sqs/configuration-options.md#region),
* `QueueNamePrefix=<value>` - Queue name prefix transport [option](/transports/sqs/configuration-options.md#queuenameprefix),
* `TopicNamePrefix=<value>` - Topic name prefix transport [option](/transports/sqs/configuration-options.md#topicnameprefix)
* `S3BucketForLargeMessages=<value>` - S3 bucket for large messages [option](/transports/sqs/configuration-options.md#s3bucketforlargemessages),
* `S3KeyPrefix=<value>` - S3 key prefic [option](/transports/sqs/configuration-options.md#s3bucketforlargemessages-s3keyprefix).
