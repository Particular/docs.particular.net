In order to enable the Outbox for transports that don't support distributed transactions, e.g. RabbitMQ transport, use the following code API:

snippet: OutboxEnablineInCode

In order to enable the Outbox for transports that support distributed transactions, e.g. MSMQ or SQL Server transport, it is additionally required to add the following `app.config` setting:

snippet: OutboxEnablingInAppConfig

Note: When Outbox is enabled then NServiceBus automatically lowers the default delivery guarantee level to `ReceiveOnly`. A different level can be explicitly [specified in configuration](/nservicebus/transports/transactions.md).

Warning: The double opt-in configuration for transports supporting DTC is ensuring that Outbox is not accidentally used in combination with DTC. If endpoints using Outbox send messages to endpoints using DTC, then messages might get duplicated. As a result, the same messages might be processed multiple times. 