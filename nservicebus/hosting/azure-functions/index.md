---
title: Azure Functions
summary: Hosting NServiceBus in Azure Functions
tags:
 - Hosting
related:
 - samples/azure/functions
versions: '[0.1,]'
reviewed: 2019-08-27
---

Azure Functions, and serverless computing, in general, is designed to accelerate and simplify application development. NServiceBus endpoints can be hosted in Azure Functions are subject to the constraints enforced by the Functions hosting and development model.


## Known constraints and limitations

- Endpoints cannot create their own infrastructure using installers. The infrastructure required by the endpoint to run needs to be created upfront.
  - Queues for commands (ASB and ASQ)
  - Topics and subscriptions for events (ASB)
  - Subscription records in storage for events (ASQ)
- No support for `SendsAtomicWithReceive` [transport transaction mode](https://docs.particular.net/transports/transactions?version=core_7.2#transactions-transport-transaction-sends-atomic-with-receive) with Azure Service Bus
- No support for triggers other than [`ServiceBusTrigger`](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-service-bus) and [`QueueTrigger`](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue)
- Configuration API exposes transport configuration options via `configuration.Transport` property that are not all applicable to the endpoints hosted in Azure Functions