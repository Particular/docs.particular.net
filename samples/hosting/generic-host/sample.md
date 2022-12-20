---
title: Endpoint hosting with the Generic Host
summary: Hosting an endpoint with the Generic Host.
reviewed: 2022-11-09
component: Core
related:
- nservicebus/hosting/extensions-hosting
---

The sample uses the Generic Host and the [`Microsoft.Extensions.Hosting.WindowsServices`](https://www.nuget.org/packages/Microsoft.Extensions.Hosting.WindowsServices/) NuGet package to host NServiceBus as a Windows Service using the Generic Host underneath.

downloadbutton

## Prerequisites

The sample has the following prerequisites:

- [.NET 4.8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- [PowerShell Core for Windows](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-windows) in case the sample is to be installed as a Windows Service

## Running the sample as a console

In Visual Studio, press <kbd>F5</kbd> to start the sample as a console application.

## Running the sample as a Windows Service

- [Install PowerShell Core on Windows](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-windows)
- Start PowerShell core with elevated permissions
- Run `dotnet publish` in the directory of the sample; for example: `C:\samples\generic-host`
- Run `New-Service -Name WorkerTest -BinaryPathName "C:\samples\generic-host\bin\Debug\net48\publish\GenericHost.exe"`
- Run `Start-Service WorkerTest`
- Go to the Event Viewer under `Windows Logs\Applications` and observe event log entries from source `GenericHost` with the following content
```
Category: MyMessageHandler
EventId: 0

Received message #{Number}
```
- Once done, run `Stop-Service WorkerTest` and `Remove-Service WorkerTest`

NOTE: Currently to use the `Microsoft.Extensions.Logging` library, the `NServiceBus.MicrosoftLogging` community package should be used.

## Code walk-through

snippet: generic-host-service-lifetime

The snippet above shows how the host builder runs by default as a Windows Service. If the sample is started with the debugger attached, it uses the console's lifetime instead. To always use the console lifetime use the following code instead:

snippet: generic-host-console-lifetime


snippet: generic-host-logging

To enable integration with Microsoft.Extensions.Logging, the [`NServiceBus.MicrosoftLogging.Hosting`](https://www.nuget.org/packages/NServiceBus.MicrosoftLogging.Hosting/) community package is used and configured in combination with the standard logging.

Next, the builder configures NServiceBus using the [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting.md) package, including the [critical error](/nservicebus/hosting/critical-errors.md) action that will shut down the application or service in case of a critical error.

snippet: generic-host-nservicebus

The critical error action:

snippet: generic-host-critical-error

To simulate work, a BackgroundService called `Worker` is registered as a hosted service:

snippet: generic-host-worker-registration

The `IMessageSession` is injected into the `Worker` constructor, and the `Worker` sends messages when it is executed.

snippet: generic-host-worker
