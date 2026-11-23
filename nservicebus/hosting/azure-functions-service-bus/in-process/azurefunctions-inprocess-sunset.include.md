> [!WARNING]
> **NServiceBus.AzureFunctions.InProcess.ServiceBus component is no longer supported**
>
> Microsoft announced that .NET 8 will be [the last release supporting the in-process hosting model](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/net-on-azure-functions-august-2023-roadmap-update/ba-p/3910098) and that [beginning November 10, 2026, the in-process model for .NET apps in Azure Functions will no longer be supported](https://azure.microsoft.com/en-us/updates?id=retirement-support-for-the-inprocess-model-for-net-apps-in-azure-functions-ends-10-november-2026). This component will also stop receiving any updates.
> New projects should use the [isolated worker model](/nservicebus/hosting/azure-functions-service-bus/) instead. Follow [these instructions](/nservicebus/upgrades/azure-functions-service-bus-in-process-isolated-worker.md) to migrate.
