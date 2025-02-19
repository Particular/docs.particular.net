---
title: Endpoint hosting with the Generic Host
summary: Hosting an endpoint with the Generic Host
reviewed: 2025-02-19
component: Core
related:
- nservicebus/hosting/extensions-hosting
---

The sample uses the Generic Host and the [`Microsoft.Extensions.Hosting.WindowsServices`](https://www.nuget.org/packages/Microsoft.Extensions.Hosting.WindowsServices/) NuGet package to host NServiceBus as a Windows Service.

## Code walk-through

The builder configures NServiceBus using the [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting.md) package, including the [critical error](/nservicebus/hosting/critical-errors.md) action that will shut down the application or service in case of a critical error.

snippet: generic-host-nservicebus

The critical error action:

snippet: generic-host-critical-error

To simulate work, a BackgroundService called `Worker` is registered as a hosted service:

snippet: generic-host-worker-registration

The `IMessageSession` is injected into the `Worker` constructor, and the `Worker` sends messages when it is executed.

snippet: generic-host-worker

## Running the sample as a Windows Service

- Start PowerShell with elevated permissions
- Run `dotnet publish` in the directory of the sample; for example: `C:\samples\generic-host`
- Run `New-Service -Name WorkerTest -BinaryPathName "C:\samples\generic-host\bin\Debug\{framework}\publish\GenericHost.exe"`
- Run `Start-Service WorkerTest`
- Go to the Event Viewer under `Windows Logs\Applications` and observe event log entries from source `GenericHost` with the following content:
```
Category: MyMessageHandler
EventId: 0

Received message #{Number}
```
- Once done, run `Stop-Service WorkerTest` and `Remove-Service WorkerTest`