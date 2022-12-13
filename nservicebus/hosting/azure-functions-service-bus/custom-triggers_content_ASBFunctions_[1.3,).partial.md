If the trigger function must be customized, disable generation of the trigger function by removing the `NServiceBusTriggerFunction` attribute. A custom trigger function can then be added manually to the project:

snippet: custom-trigger-definition

## Configuring transaction mode

To use the `TransportTransactionMode.SendsAtomicWithReceive` mode the auto-complete needs to be **disabled** and the trigger needs to call `ProcessAtomic`. 

snippet: asb-function-message-consistency-process-transactionally

To use the `TransportTransactionMode.ReceiveOnly` mode the auto-complete needs to be **enabled** and the trigger needs to call `ProcessNonAtomic`. 

snippet: asb-function-message-consistency-process-non-transactionally

DANGER: Incorrectly configuring the service bus trigger auto-complete setting can lead to message loss. Use the auto-detection mechanism on the function endpoint interface, or use the trigger function attribute to specify message consistency.
