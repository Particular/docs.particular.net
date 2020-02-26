---
title: Endpoint hosting with the Generic Host
summary: Hosting an endpoint with the Generic Host.
reviewed: 2020-02-28
component: Core
tags:
- Hosting
related:
- nservicebus/hosting
- nservicebus/hosting/assembly-scanning
---

## Code walk-through

The sample uses the Generic Host and the [`Microsoft.Extensions.Hosting.WindowsServices`](https://www.nuget.org/packages/Microsoft.Extensions.Hosting.WindowsServices/) nuget package to host NServiceBus as a console application or as a Windows Service using the Generic Host underneath.

snippet: generic-host-lifetime

The snippet above shows how the host builder runs by default as Windows Service in case the debugger is not attached or the `console` parameter is provided as input argument.

snippet: generic-host-logging

To enable integration with Microsoft.Extensions.Logging the [`NServiceBus.MicrosoftLogging.Hosting`](https://www.nuget.org/packages/NServiceBus.MicrosoftLogging.Hosting/) community package is used and configured in combination with the standard logging.

Next up the builder configures the usage of NServiceBus, this is achieved with the [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting.md) package, including the [critical error](/nservicebus/hosting/critical-errors.md) action that will shutdown the application or service in case of a critical error.

snippet: generic-host-nservicebus

The critical error action:

snippet: generic-host-critical-error

To simulate work a BackgroundService called `Worker` is registered as a hosted services:

snippet: generic-host-worker-registration

The worker takes a dependency to `IServiceProvider` to be able to retrieve the message session. This is required because all hosted services are resolved from the container first and then started in the order of having been added. Therefore it is not possible to inject `IMessageSession` directly because the hosted service that starts the NServiceBus endpoint has not been started yet when the worker service constructor is being resolved from the container.

snippet: generic-host-worker

### Running the sample

Hit F5 to start the sample as a console application.

### Running the sample as a Windows Service

- [Install PowerShell Core on Windows](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-windows?view=powershell-7)
- Start Powershell core with elevated permissions
- Run `dotnet publish` in the directory of the sample, let's say `C:\samples\generic-host`
- Run `New-Service -Name WorkerTest -BinaryPathName "C:\samples\generic-host\bin\Debug\netcoreapp3.1\win-x64\publish\GenericHost.exe" | Start-Service`
- Run `Start-Service WorkerTest`
- Go to the Event Viewer under `Windows Logs\Applications` and observe event log entries from source `GenericHost` with the following content
```
Category: MyMessageHandler
EventId: 0

Received message #{Number}
```
- Once done, run `Stop-Service WorkerTest` and `Remove-Service WorkerTest`