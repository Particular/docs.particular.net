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

Since Microsoft has confirmed that .Net 8.0 will be the last LTS to support the in-process hosting model, it is highly recommended to migrate to the isolated worker hosting model.  The following sections describe how to migrate Azure Functions (in-process) to (Isolated Worker).

The main difference between Azure Functions (in-process) and Azure Functions (Isolated Worker) is that Isolated Worker functions are isolated from the Azure Functions runtime, while in-process functions share the same process and AppDomain. Microsoft has more detailed information about the [difference between the in-process and isolated worker model](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-in-process-differences).

## Update project file

### Update framework

- Change the `TargetFramework` to `net6.0` or greater

### Remove package references

- `Microsoft.NET.Sdk.Functions`
- `Microsoft.Azure.WebJobs`
- `Microsoft.Azure.Webjobs`
- `Micrsosoft.Azure.Functions.Exension`
- `Microsoft.Azure.Webjobs`
- `Microsoft.Azure.Webjobs.extensions.http`

### Add package references

- `Microsoft.Azure.Functions.Worker` >= 1.0.0
- `Microsoft.Azure.Functions.Worker.Sdk` >= 1.0.0
- `Microsoft.Azure.Functions.Worker.Extensions.Http` >= 3.0.0
- `NServiceBus.AzureFunctions.Worker.ServiceBus` >= 3.0.0

## In HttpSender.cs

### Remove references

- `Microsoft.AspNetCore.Http`
- `Microsoft.AspNetCore.Mvc`
- `Microsoft.Azure.WebJobs.Http`
- `Microsoft.Azure.WebJobs`
- `Microsoft.Azure.WebJobs.Extensions.Http`

### Add references

- `Microsoft.Azure.Functions.Worker`
- `Microsoft.Azure.Functions.Worker.Extensions.Http`

### Change return type of Run method

- Change from `Task<IActionResult>` to `Task<HttpResponseData>`
- Change `ExecutionContext` parameter to `FunctionContext`
- The functionEndpoint.send method now does not take the logger as a parameter.

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

## In Startup.cs or program.cs

Simple example:

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

Utilizing the minimal hosting model available in .Net 6.0, the `startup.cs` file can be merged with the `program.cs` file to register services.

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
