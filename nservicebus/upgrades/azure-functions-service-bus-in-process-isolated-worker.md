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
> When migrating from the in-process model to the isolated worker model, it is highly recommended that you follow [Microsoft's migration guide](https://learn.microsoft.com/en-us/azure/azure-functions/migrate-dotnet-to-isolated-model?tabs=net8) to start with, as this guide specifically covers the NServiceBus functionality.

## Key differences

Find more detailed information about the [difference between the in-process and isolated worker model](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-in-process-differences).

- **In-process**: Functions run within the Azure Functions runtime, share the same process an AppDomain.
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

```csharp
[assembly: NServiceBusTriggerFunction(Program.EndpointName)]

public class Program
{
  public const string EndpointName = "WorkerDemoEndpoint";

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

## Update trigger functions

Remember to follow Microsoft's migration guide to update the [trigger functions and bindings](https://learn.microsoft.com/en-us/azure/azure-functions/migrate-dotnet-to-isolated-model?tabs=net8#trigger-and-binding-changes).

### Trigger function signature and logic

- Change `ExecutionContext` parameter to `FunctionContext`
- The logger can be accessed through the `FunctionContext`
- The `functionEndpoint.Send` method no longer takes the logger as a parameter.
- Update the trigger function annotation from `[FunctionName(<triggerFunctionName>)` to `[Function(<triggerFunctionName>)]`

An example of an in-process trigger function

```csharp
 public class HttpSender
 {
     readonly IFunctionEndpoint functionEndpoint;

     public HttpSender(IFunctionEndpoint functionEndpoint)
     {
         this.functionEndpoint = functionEndpoint;
     }

     [FunctionName("HttpSender")]
     public async Task<IActionResult> Run(
         [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest request, ExecutionContext executionContext, ILogger logger)
     {
         logger.LogInformation("C# HTTP trigger function received a request.");

         var sendOptions = new SendOptions();
         sendOptions.RouteToThisEndpoint();

         await functionEndpoint.Send(new TriggerMessage(), sendOptions, executionContext, logger);

         return new OkObjectResult($"{nameof(TriggerMessage)} sent.");
     }
 }
```

This is an example of a trigger function migrated to the isolated worker model

```csharp
class HttpSender
{
    readonly IFunctionEndpoint functionEndpoint;

    public HttpSender(IFunctionEndpoint functionEndpoint)
    {
        this.functionEndpoint = functionEndpoint;
    }

    [Function("HttpSender")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req,
        FunctionContext functionContext)
    {
        var logger = functionContext.GetLogger<HttpSender>();
        logger.LogInformation("C# HTTP trigger function received a request.");

        var sendOptions = new SendOptions();
        sendOptions.RouteToThisEndpoint();

        await functionEndpoint.Send(new TriggerMessage(), sendOptions, functionContext);

        var r = req.CreateResponse(HttpStatusCode.OK);
        await r.WriteStringAsync($"{nameof(TriggerMessage)} sent.");
        return r;
    }
}
```
