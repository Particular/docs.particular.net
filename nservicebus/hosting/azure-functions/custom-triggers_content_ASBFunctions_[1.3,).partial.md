If the trigger function needs to be customized, the trigger function generation can be disabled by removing the `NServiceBusTriggerFunction` attribute. A customized trigger function can then be manually added to the project:

snippet: custom-trigger-definition

## Configuring transaction mode

If the `NServiceBusTriggerFunction` attribute is not being used, `IFunctionEndpoint.Process` will determine the transaction mode based on the `ServiceBusTrigger`'s `AutoComplete` property:

If auto-complete is **enabled**, which is the default, then NServiceBus cannot control the receive transaction and the message is processed in `TransportTransactionMode.ReceiveOnly` mode.

snippet: asb-function-message-consistency-process-non-transactionally

If auto-complete is **disabled**, then NServiceBus can fully control incoming and outgoing messages and the message is processed in `TransportTransactionMode.SendsAtomicWithReceive` mode.

snippet: asb-function-message-consistency-process-transactionally

If additional control is required, or the service bus triger is not configured using an attribute, use the concrete `FunctionEndpoint` class:

snippet: asb-function-message-consistency-manual

DANGER: Incorrectly configuring the service bus trigger auto-complete setting can lead to message loss. It is recommended to use the auto-detection mechanism on the function endpoint interface, or to use the trigger function attribute to specify message consistency.