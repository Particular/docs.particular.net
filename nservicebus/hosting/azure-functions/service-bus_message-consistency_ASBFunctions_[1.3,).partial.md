NServiceBus can provide transactional consistency between incoming and outgoing messages:

snippet: asb-function-enable-sends-atomic-with-receive-with-attribute

This is the equivalent to the [`SendsAtomicWithReceive`](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) transport transaction mode. By default, transactional consistency is disabled, providing the same transport guarantees as the [`ReceiveOnly`](/transports/transactions.md#transactions-transport-transaction-receive-only) transport transaction mode.

### Controlling consistency with custom trigger defintion

If the `NServiceBusTriggerFunction` attribute is not being used, `IFunctionEndpoint.Process` will determine the transaction mode based on the `ServiceBusTrigger`'s `AutoComplete` property:

If auto-complete is **enabled**, which is the default, then NServiceBus cannot control the receive transaction and the message is processed in `TransportTransactionMode.ReceiveOnly` mode.

snippet: asb-function-message-consistency-process-non-transactionally

If auto-complete is **disabled**, then NServiceBus can fully control incoming and outgoing messages and the message is processed in `TransportTransactionMode.SendsAtomicWithReceive` mode.

snippet: asb-function-message-consistency-process-transactionally

If additional control is required, or the service bus triger is not configured using an attribute, use the concrete function endpoint class.

snippet: asb-function-message-consistency-manual

DANGER: Incorrectly configuring the service bus trigger auto-complete setting can lead to message loss. It is recommended to use the auto-detection mechanism on the function endpoint interface, or to use the trigger function attribute to specify message consistency.
