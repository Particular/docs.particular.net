If the trigger function must be customized, disable generation of the trigger function by removing the `NServiceBusTriggerFunction` attribute. A custom trigger function can then be added manually to the project:

snippet: custom-trigger-definition

## Configuring transaction mode

If the `NServiceBusTriggerFunction` attribute is not used, `IFunctionEndpoint.Process` will determine the transaction mode based on the `ServiceBusTrigger`'s auto-complete property:

If auto-complete is **enabled**, which is the default, NServiceBus can't control the receive transaction and the message is processed in `TransportTransactionMode.ReceiveOnly` mode.

snippet: asb-function-message-consistency-process-non-transactionally

If auto-complete is **disabled**, NServiceBus can fully control incoming and outgoing messages and the message is processed in `TransportTransactionMode.SendsAtomicWithReceive` mode.

snippet: asb-function-message-consistency-process-transactionally

DANGER: Incorrectly configuring the service bus trigger auto-complete setting can lead to message loss. Use the auto-detection mechanism on the function endpoint interface, or use the trigger function attribute to specify message consistency.
