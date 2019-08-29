---
title: Azure Functions
summary: Hosting NServiceBus in Azure Functions
tags:
 - Hosting
related:
 - samples/azure/functions
reviewed: 2019-08-28
---

Azure Functions, and serverless computing in general, is designed to accelerate and simplify application development. NServiceBus endpoints can be hosted in Azure Functions are subject to the constraints enforced by the Functions hosting and development model.


## Known constraints and limitations

When using Azure Functions with Azure Service Bus (ASB) or Azure Storage Queues (ASQ), the following needs to be taken into consideration:

- Endpoints cannot create their own queues or other infrastructure using installers. The infrastructure required by the endpoint to run must be created upfront.
  - Queues for commands (ASB and ASQ)
  - Topics and subscriptions for events (ASB)
  - Subscription records in storage for events (ASQ)
- No support for `SendsAtomicWithReceive` [transport transaction mode](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) with Azure Service Bus.
- Supported triggers are:
  -  [`ServiceBusTrigger`](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus) for Azure Service Bus
  - [`QueueTrigger`](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue) for Azure Storage Queues
  - Other triggers will not result in the invocation of the NServiceBus pipeline, but could be used with a send-only endpoint to translate a function invocation into a command.
- Configuration API exposes NServiceBus transport configuration options via the `configuration.Transport` to allow customization, however not all the available options will be applicable to execution within Azure Functions.
- The NServiceBus `ILog` logigng abstraction and the Azure Functions `ILogger` are not wired to work together.
- While the Functions runtime uses the default connection string in the app setting that is named "AzureWebJobsServiceBus" (ASB) or "AzureWebJobsStorage" (ASQ), there's a [bug](https://github.com/Azure/azure-functions-servicebus-extension/issues/7) with the Service Bus and the trigger connection has to be specified.
- When using the default recoverability or specifying custom number of immediate retries, the number of delivery attempts specified on the underlying queue (ASB) or Functions host (ASB) must be more than then number of the immediate retries. The Functions defaults are 10 (`MaxDeliveryCount`) for the ASB trigger and 5 (`DequeueCount`) for the ASQ trigger.
