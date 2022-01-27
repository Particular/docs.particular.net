{{NOTE: Microsoft.Azure.WebJobs.Extensions.ServiceBus Version 5 requires [`EnableCrossEntityTransactions`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.webjobs.servicebus.servicebusoptions.enablecrossentitytransactions) to be enabled in order to support sends atomic with receive.

This can be done by adding the following to `host.json`

```
"extensions": {
    "ServiceBus": {
      "EnableCrossEntityTransactions": true
    }
  }
```
}}