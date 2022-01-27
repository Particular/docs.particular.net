NServiceBus can provide transactional consistency between incoming and outgoing messages:

snippet: asb-function-enable-sends-atomic-with-receive-with-attribute

This is equivalent to the [`SendsAtomicWithReceive`](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) transport transaction mode. By default, transactional consistency is disabled, providing the same transport guarantees as the [`ReceiveOnly`](/transports/transactions.md#transactions-transport-transaction-receive-only) transport transaction mode.

For more information on configuring message consistency using custom triggers, refer to the [custom Azure Functions triggers](/nservicebus/hosting/azure-functions-service-bus/custom-triggers.md) documentation.

include: servicebus_options_enable_cross_entity_transactions
