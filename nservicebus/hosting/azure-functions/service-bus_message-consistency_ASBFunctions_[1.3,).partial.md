NServiceBus can provide transactional consistency between incoming and outgoing messages:

snippet: asb-function-enable-sends-atomic-with-receive-with-attribute

This is the equivalent to the [sends atomic with receive](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) transport transaction mode.

### Without trigger function attribute

If the Azure Function queue trigger attribute is not being used, then NServiceBus will process messages transactionally if it can control the receive transaction. This is done by looking for `ServiceBusTriggerAttribute` in the call stack and checking the `AutoComplete` property.

If auto-complete is enabled, which is the default, then NServiceBus cannot control the receive transaction and the message is processed non-transactionally.

snippet: asb-function-message-consistency-process-non-transactionally

If auto-complete is not enabled then NServiceBus can control the receive transaction and the message is processed transactionally.

snippet: asb-function-message-consistency-process-transactionally

If additional control is required, or the service bus triger is not configured using an attribute, use the concrete function endpoint class.

snippet: asb-function-message-consistency-manual

DANGER: Incorrectly configuring the service bus trigger auto-complete setting can lead to message loss. It is recommended to use the auto-detection mechanism on the function endpoint interface, or to use the trigger function attribute to specify message consistency.