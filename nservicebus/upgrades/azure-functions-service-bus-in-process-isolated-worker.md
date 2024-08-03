---
title: Migrating Azure Functions in-process to Isolated Worker
summary: Instructions on how to migrate Azure Functions in-process to Azure Functions Isolated Worker
component: ASBFunctionsWorker
reviewed: 2024-07-29
related:
 - nservicebus/hosting/azure-functions-service-bus/in-process
 - samples/azure-functions/service-bus
 - nservicebus/hosting/azure-functions-service-bus
 - samples/azure-functions/service-bus-worker
isUpgradeGuide: false
---

Microsoft has confirmed that .NET 8.0 will be the last LTS to support the in-process hosting model and .NET 6.0 LTS will reach end of support in November 2024. Migrating to the isolated worker hosting model is strongly advised.

> [!NOTE]
> When migrating from the in-process model to the isolated worker model, please follow [Microsoft's migration guide](https://learn.microsoft.com/en-us/azure/azure-functions/migrate-dotnet-to-isolated-model?tabs=net8) to begin with, as this guide will focus on the NServiceBus functionality.

## Key differences

Find more detailed information about the [difference between the in-process and isolated worker model](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-in-process-differences).

- **In-process**: Functions run within the Azure Functions runtime, share the same process and AppDomain.
- **Isolated worker**: Functions are isolated from the Azure Functions runtime and run in a separate process.

## Update project file

### Framework

It's recommended to update the `TargetFramework` in your `.csproj` file to `net8.0`.

### Package references

- Remove `NServiceBus.AzureFunctions.InProcess.ServiceBus` from your `.csproj` file.
- Add `NServiceBus.AzureFunctions.Worker.ServiceBus` to your `.csproj` file with the appropriate version based on the version of NServiceBus in your project.

## Update host configuration

A `Program.cs` file is required when migrating to the isolated worker model.

This is an example of and in-process host configuration using `Startup.cs`

```csharp
[assembly: FunctionsStartup(typeof(Startup))]
[assembly: NServiceBusTriggerFunction(Startup.EndpointName)]

class Startup : FunctionsStartup
{
    public const string EndpointName = "InProcessDemoEndpoint";

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.UseNServiceBus();
    }
}
```

A migrated host configuration for isolated worker to `Program.cs` would look like the following

snippet: asb-function-isolated-configuration

## Update trigger functions

Remember to follow Microsoft's migration guide to update the [trigger functions and bindings](https://learn.microsoft.com/en-us/azure/azure-functions/migrate-dotnet-to-isolated-model?tabs=net8#trigger-and-binding-changes).

### Trigger function signature and logic

- Change `ExecutionContext` parameter to `FunctionContext`
- The `functionEndpoint.Send` method now does not take the logger as a parameter.
- Update the trigger function annotation from `[FunctionName(<triggerFunctionName>)` to `[Function(<triggerFunctionName>)]`

An example of an in-process trigger function

```csharp
class HttpTrigger
{
    IFunctionEndpoint functionEndpoint;

    public HttpTrigger(IFunctionEndpoint functionEndpoint)
    {
        this.functionEndpoint = functionEndpoint;
    }

    [FunctionName("HttpSender")]
    public Task Run(
        [ServiceBusTrigger("MyFunctionsEndpoint")]
        ServiceBusReceivedMessage message,
        ServiceBusClient client,
        ServiceBusMessageActions messageActions,
        ILogger logger,
        ExecutionContext executionContext)
    {
        return functionEndpoint.ProcessAtomic(message, executionContext, client, messageActions, logger);
    }
}
```

This is an example of a trigger function migrated to the isolated worker model

snippet: asb-function-isolated-dispatching-outside-message-handler
