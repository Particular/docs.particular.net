---
title: Custom Azure Functions triggers
component: ASBFunctions
summary: How to write custom triggers to call NServiceBus from Azure Functions
related:
 - nservicebus/hosting/azure-functions-service-bus/in-process
reviewed: 2024-08-02
---

include: azurefunctions-inprocess-sunset

To configure a custom trigger function, remove the native `NServiceBusTriggerFunction` attribute. A custom trigger function can then be added manually to the project:

snippet: custom-trigger-definition

## Configuring transaction mode

To use the `TransportTransactionMode.SendsAtomicWithReceive` mode, `AutoCompleteMessages` needs to be **disabled** and the custom trigger needs to call `ProcessAtomic`.

snippet: asb-function-message-consistency-process-transactionally

To use the `TransportTransactionMode.ReceiveOnly` mode, `AutoCompleteMessages` needs to be **enabled** and the trigger needs to call `ProcessNonAtomic`.

snippet: asb-function-message-consistency-process-non-transactionally

> [!WARNING]
> Incorrectly configuring the service bus trigger `AutoCompleteMessages` setting can lead to message loss. Use the auto-detection mechanism on the function endpoint interface, or use the trigger function attribute to specify message consistency.
