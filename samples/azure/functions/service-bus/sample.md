---
title: Using NServiceBus in Azure Functions
reviewed: 2019-08-23
component: ASBFunctions
---

Command to create Azure Functions trigger queue using Azure CLI

```
az servicebus queue create --name <function-name> --namespace-name <asb-namespace-to-use> --resource-group <resource-group-containing-namespace>
```