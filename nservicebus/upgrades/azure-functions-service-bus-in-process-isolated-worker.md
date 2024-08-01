---
title: Migrating Azure Functions (in-process) to (Isolated Worker)
summary: Instructions on how to migrate Azure Functions (in-process) to Azure Functions (Isolated Worker)
component: ASBFunctionsWorker
reviewed: 2024-07-29
related:
 - nservicebus/hosting/azure-functions-service-bus
 - samples/azure-functions/service-bus-worker
 - nservicebus/hosting/azure-functions-service-bus/in-process
 - samples/azure-functions/service-bus
isUpgradeGuide: false
---

## Overview

Microsoft has confirmed that .NET 8.0 will be the last LTS to support the in-process hosting model and .NET 6.0 LTS will reach end of support in November 2024. It is highly recommended to migrate to the isolated worker hosting model.  This guide provides steps to help you migrate from in-process to isolated worker.

## Key differences

- **In-process**: Functions run within the Azure Functions runtime, sharing the same process and AppDomain.
- **Isolated worker**: Functions are isolated from the Azure Functions runtime and run in a separate process.

Microsoft provides more detailed information about the [difference between the in-process and isolated worker model](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-in-process-differences).

## Update project file

### Update framework

It's recommended to update the `TargetFramework` in your `.csproj` file to `net8.0`.  However, NServiceBus.AzureFunctions.Worker.ServiceBus (>= 1.0.0 && < 5.0.0) will work with `net6.0`.

```xml
<PropertyGroup>
  <TargetFrameworks>net8.0</TargetFrameworks>
</PropertyGroup>
```

### Remove package references

Remove the following package references from your `.csproj` file.

- `Microsoft.NET.Sdk.Functions`
- `Micrsosoft.Azure.Functions.Exension`
- `Microsoft.Azure.Webjobs`
- `Microsoft.Azure.Webjobs.extensions.http`
- `NServiceBus.AzureFunctions.InProcess.ServiceBus`

### Add package references

Add the following package references to your `.csproj` file with the appropriate dependency version.

- `Microsoft.Azure.Functions.Worker`
- `Microsoft.Azure.Functions.Worker.Sdk`
- `Microsoft.Azure.Functions.Worker.Extensions`
- `NServiceBus.AzureFunctions.Worker.ServiceBus`

## Update trigger function

Remove the following references:

- `Microsoft.AspNetCore.Http`
- `Microsoft.AspNetCore.Mvc`
- `Microsoft.Azure.WebJobs`
- `Microsoft.Azure.WebJobs.Http`
- `Microsoft.Azure.WebJobs.Extensions.Http`

Add the following references

- `Microsoft.Azure.Functions.Worker`
- `Microsoft.Azure.Functions.Worker.Extensions.Http`

### Update trigger function signature and logic

- Change from `Task<IActionResult>` to `Task<HttpResponseData>`
- Change `ExecutionContext` parameter to `FunctionContext`
- The `functionEndpoint.Send` method now does not take the logger as a parameter.
- Update the trigger function annotation from `[FunctionName(<triggerFunctionName>)` to `[Function(<triggerFunctionName>)]`

```csharp
[Function("TriggerFunctionName")]
public async Task<HttpResponseData> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
    HttpRequestData req,
    FunctionContext executionContext)
{
    var logger = executionContext.GetLogger<HttpSender>();
    logger.LogInformation("C# HTTP trigger function received a request.");

    var sendOptions = new SendOptions();
    sendOptions.RouteToThisEndpoint();

    await functionEndpoint.Send(new TriggerMessage(), sendOptions, executionContext);

    var r = req.CreateResponse(HttpStatusCode.OK);
    await r.WriteStringAsync($"{nameof(TriggerMessage)} sent.");
    return r;
}
```

## Update host configuration

Utilizing the minimal hosting model that became available in .Net 6.0, the `startup.cs` file can be merged with the `program.cs` file to register services.

### Simple example

snippet: asb-function-isolated-configuration

### With dependency injection

```csharp
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using System.Threading.Tasks;

[assembly: NServiceBusTriggerFunction("ASBWorkerEndpoint")]

public class Program
{
    public static Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults(builder =>
            {
                // Register custom service in the container
                builder.Services.AddSingleton(serviceProvider =>
                {
                    var configurationRoot = serviceProvider.GetRequiredService<IConfiguration>();
                    var customComponentInitializationValue = configurationRoot.GetValue<string>("CustomComponentValue");

                    return new CustomComponent(customComponentInitializationValue);
                });
            })
            .UseNServiceBus()
            .Build();

        return host.RunAsync();
    }
}
```
