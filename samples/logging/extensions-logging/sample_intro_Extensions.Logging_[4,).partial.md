This sample shows how to configure an NServiceBus endpoint to use NLog through the `Microsoft.Extensions.Logging` abstractions.

Both Microsoft.Extensions.Logging and NServiceBus.Logging are logging abstractions and both must be configured.

The following logging chain is created:

- NServiceBus.Logging
  - Microsoft.Extensions.Logging
    - NLog.Extensions.Logging
      - NLog
        - Console output

> [!NOTE]
> Starting with NServiceBus 10.2, endpoints hosted with the [.NET Generic Host](/nservicebus/hosting/core-hosting.md) using `AddNServiceBusEndpoint` automatically integrate with `Microsoft.Extensions.Logging` and the `NServiceBus.Extensions.Logging` bridge package is no longer required. This sample demonstrates the modern approach.
