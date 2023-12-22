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

partial: code-walk-through
