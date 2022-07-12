In order to enable the Outbox, use the following code API:

snippet: OutboxEnablineInCode

Note: When Outbox is enabled then NServiceBus automatically lowers the default delivery guarantee level to `ReceiveOnly`. A different level can be explicitly [specified in configuration](/transports/transactions.md).