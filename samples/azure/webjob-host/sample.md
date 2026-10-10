---
title: Self-Hosting in Azure WebJobs
summary: Host an NServiceBus endpoint in Azure WebJobs.
component: Core
reviewed: 2026-10-09
isLearningPath: true
redirects:
- samples/azure/self-host
- samples/azure/shared-host
---

This is an example of how an NServiceBus endpoint can be hosted using Azure WebJobs. This sample is compatible with Azure WebJobs SDK 3.0.

## Running in development mode

 1. Start a current release of the [Azurite Storage Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite).
 2. Run the solution.

> [!NOTE]
> If startup fails because an Azure Storage API version is not supported by Azurite, update the emulator. Alternatively, start Azurite with `--skipApiVersionCheck` to bypass its API-version check.

## Code walk-through

This sample contains one project:

- Receiver: A self-hosted endpoint running in a continuous WebJob.

### Receiver

The receiver uses the self-hosting capability to start an endpoint inside a WebJob.

#if-version [10, )

Register the endpoint on the host's service collection with `AddNServiceBusEndpoint`. This [built-in hosting integration](/nservicebus/hosting/core-hosting.md) starts and stops the endpoint with the host:

#end-if

#if-version [, 10)

The `UseNServiceBus` method of [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting.md) configures and starts the endpoint:

#end-if

snippet: WebJobHost_Start

> [!NOTE]
> If dependencies need to be shared between the service collection and NServiceBus infrastructure (e.g., message handlers), refer to the [ASP.NET Core sample](/samples/dependency-injection/aspnetcore).

A [critical error](/nservicebus/hosting/critical-errors.md) action must be defined to restart the host when a critical error occurs:

snippet: WebJobHost_CriticalError

When the WebJob host stops, the NServiceBus endpoint stops automatically.
