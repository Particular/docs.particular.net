 ### Token-credentials

 * `CustomTokenCredential(TokenCredential)`: Enables using Microsoft Entra ID authentication such as [managed identities for Azure resources](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-managed-service-identity) instead of the shared secret in the connection string.

> [!NOTE]
> **Microsoft Entra ID** authentication requires a fully-qualified namespace usage (e.g. `<asb-namespace-name>.servicebus.windows.net`) instead of a connection string (e.g. `Endpoint=sb://<asb-namespace-name>.servicebus.windows.net>;[...]`).
