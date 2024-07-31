---
title: Migrating Azure Functions (in-process) to (Isolated Worker)
summary: Instructions on how to migrate Azure Functions (in-process) to Azure Functions (Isolated Worker)
component: ASBFunctionsWorker
reviewed: 2024-07-29
related:
 - nservicebus/hosting/azure-functions-service-bus/in-process
 - nservicebus/hosting/azure-functions-service-bus
isUpgradeGuide: true
---

## Overview

Microsoft has confirmed that .NET 8.0 will be the last LTS to support the in-process hosting model. It is highly recommended to migrate to the isolated worker hosting model.  This guide provides steps to help you migrate from in-process to isolated worker.

## Key differences

- **In-process**: Functions run within the Azure Functions runtime, sharing the same process and AppDomain.
- **Isolated worker**: Functions are isolated from the Azure Functions runtime and run in a separate process.

Microsoft provides more detailed information about the [difference between the in-process and isolated worker model](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-in-process-differences).

This migration guide is based on the `NServicebus.AzureFunctions.InProcess.ServieBus 2.x` component [example](/samples/azure-functions/service-bus/?version=asbfunctions_2) and the `NServiceBus.AzureFunctions.Worker.ServiceBus 3.x` component [example](/samples/azure-functions/service-bus-worker/?version=asbfunctionsworker_3) as reference.

## Update project file

### Update framework

  Change the `TargetFramework` to `net6.0` or higher in your `.csproj` file

```xml
<PropertyGroup>
  <TargetFramework>net6.0</TargetFramework>
</PropertyGroup>
```

### Remove package references

Remove the following package references from your `.csproj` file.

- `Microsoft.NET.Sdk.Functions`
- `Micrsosoft.Azure.Functions.Exension`
- `Microsoft.Azure.Webjobs`
- `Microsoft.Azure.Webjobs.extensions.http`

### Add package references

Add the following package references to your `.csproj` file.

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Azure.Functions.Worker" Version="1.*" />
  <PackageReference Include="Microsoft.Azure.Functions.Worker.Sdk" Version="1.*" />
  <PackageReference Include="Microsoft.Azure.Functions.Worker.Extensions.Http" Version="3.*" />
  <PackageReference Include="NServiceBus.AzureFunctions.Worker.ServiceBus" Version="3.*" />
</ItemGroup>
```

## Update function code

### Modify `HttpSender.cs`

Remove the following references:

- `Microsoft.AspNetCore.Http`
- `Microsoft.AspNetCore.Mvc`
- `Microsoft.Azure.WebJobs.Http`
- `Microsoft.Azure.WebJobs`
- `Microsoft.Azure.WebJobs.Extensions.Http`

Add the following references

- `Microsoft.Azure.Functions.Worker`
- `Microsoft.Azure.Functions.Worker.Extensions.Http`

### Update the `Run` method signature and logic

- Change from `Task<IActionResult>` to `Task<HttpResponseData>`
- Change `ExecutionContext` parameter to `FunctionContext`
- The `functionEndpoint.send` method now does not take the logger as a parameter.

```csharp
[Function("HttpSender")]
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

Utilizing the minimal hosting model available in .Net 6.0, the `startup.cs` file can be merged with the `program.cs` file to register services.

### Simple example

```csharp
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System.Threading.Tasks;

[assembly:NServiceBusTriggerFunction("ASBWorkerEndpoint")]

public class Program
{
    public static Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus()
            .Build();

        return host.RunAsync();
    }
}
```

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
