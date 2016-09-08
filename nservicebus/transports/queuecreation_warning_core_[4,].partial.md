
WARN: NServiceBus will automatically request the transport to create queues needed if the [installers](/nservicebus/operations/installers.md) are enabled. This also includes queues needed by all declared [satellites](/nservicebus/satellites). Prefer the use of scripts to create custom queues instead of relying on the `IWantQueuesCreated` interface provided by NServiceBus.

The scripting guidelines shows how to take full control over queue creation:

 * [SqlServer](/nservicebus/sqlserver/operations-scripting.md#create-queues)
 * [MSMQ](/nservicebus/msmq/operations-scripting.md#create-queues)
 * [RabbitMQ](/nservicebus/rabbitmq/operations-scripting.md#create-queues)
 * [Azure ServiceBus](/nservicebus/azure-service-bus/operational-scripting.md)
