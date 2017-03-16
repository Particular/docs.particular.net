---
title: Using NServiceBus in an ASP.NET Core WebAPI Application
summary: Illustrates how to send messages to a NServiceBus endpoint from a ASP.NET Core WebAPI application.
component: Core
reviewed: 2016-09-27
related:
- nservicebus/hosting
---


This sample shows how to send messages to an NServiceBus endpoint from an ASP.NET Core WebAPI application. 

WARNING: This sample runs on the full .NET framework 4.6.1 but utilizes both .NET Core and .NET Framework dependencies, i.e. ASP.NET Core and NServiceBus. So while .NET Core dependencies can be consumed by the .NET Framework runtime, the .NET Core runtime is not currently supported.


## Converting to the full .NET Framework runtime

If the default *ASP.NET Core Web Application .NET Core* Visual Studio Project template is used, there are two actions that will be required prior to adding the NServiceBus dependency. 


### Change the target framework

To reference both .NET Core and .NET Framework dependencies, a project needs to have the framework configured to `net461`.

If this is not done, the following build error will occur

```no-highlight
NU1002 The dependency NServiceBus does not support framework .NETCoreApp,Version=v1.0.project.json
``` 

To convert to .NET Framework 4.6.1, open the project.json file and locate the following

```json
"frameworks": {
 "netcoreapp1.0": {
   "imports": [
      "dotnet5.6",
      "portable-net45+win8"
    ]
  }
},
```

Replace those lines with

```json
"frameworks": {
  "net461": {}
},
```


### Change the NuGet dependencies


#### Update existing Nuget packages

The project template will default to the following dependencies

 * Microsoft.NETCore.App 1.0
 * Microsoft.AspNetCore.\* 1.0
 * Microsoft.Extensions.\* 1.0

These are not compatible with .NET Framework 4.6.1 and will need to be updated. Use one of the following to update those packages to at least 1.1.

 * [NuGet Package Manager UI - Updating a package](https://docs.nuget.org/ndocs/tools/package-manager-ui#updating-a-package)
 * [NuGet Package Manager Console - Updating a package](https://docs.nuget.org/ndocs/tools/package-manager-console#updating-a-package)


#### Add IIS integration

Add the [Microsoft.AspNetCore.Server.IISIntegration](https://www.nuget.org/packages/Microsoft.AspNetCore.Server.IISIntegration/) NuGet pacakge. It is required for the .NET Framework runtime to host a .NET Core website in IIS.


#### Remove Microsoft.NETCore.App 

The [Microsoft.NETCore.App](https://www.nuget.org/packages/Microsoft.NETCore.App/) NuGet package is not compatible with the .NET Framework runtime. However, it is not required for this sample and can be removed.

Remove the following from the project.json file.

```json
"Microsoft.NETCore.App": {
  "version": "1.1.0",
  "type": "platform"
}
```


## Running the solution

When the solution is run, a new browser window/tab opens, as well as a console application. The browser will navigate to `http://localhost:51863/api/sendmessage`.

An async [WebAPI](https://www.asp.net/web-api) controller handles the request. It creates an NServiceBus message and sends it to the endpoint running in the console application. The message has been processed successfully when the console application prints "Message received at endpoint". 


## Prerequisites

- Install [.NET Core](https://www.microsoft.com/net/core#windows)


### Initialize the WebAPI endpoint

Open `Startup.cs` and look at the `ConfigureServices` method.

A [Send-Only endpoint](/nservicebus/hosting/#self-hosting-send-only-hosting) named `Samples.ASPNETCore.Sender` is configured in the WebAPI application by creating a new `EndpointConfiguration`.

snippet:EndpointConfiguration

Routing is configured to send every message from the assembly containing `MyMessage` to the `Samples.ASPNETCore.Endpoint` endpoint.

snippet:Routing

The endpoint is started. At this point, the configuration is locked.

snippet:EndpointStart

Finally, the endpoint is registered as a singleton instance of type `IMessageSession` in ASP.NET Cores `ServiceCollection`, ready to be injected into the controller.

An alternative would be to register the instance as type `IEndpointInstance`. `IMessageSession` is a leaner interface, containing only the methods necessary to send/publish messages. It is a good choice for [sending messages outside message handlers](/nservicebus/upgrades/5to6/moving-away-from-ibus.md#migrating-away-from-ibus-sending-messages-outside-message-handlers) if no endpoint management functionality is required.

snippet:ServiceRegistration


### Injection into the Controller

The endpoint instance is injected into the `SendMessageController` at construction time by ASP.NET Core.

snippet:MessageSessionInjection


### Sending the message 

Send and await messages through the `IMessageSession` instance provided by ASP.NET Core.

snippet:MessageSessionUsage


### Processing the message 

The message is picked up and processed by a message handler in the `Samples.ASPNETCore.Endpoint` endpoint.

snippet:MessageHandler
