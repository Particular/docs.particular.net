---
title: Self-Hosting in Azure WebJobs
summary: Use NServiceBus with self-hosting to host an endpoint in an Azure WebJob.
component: Core
reviewed: 2025-03-06
isLearningPath: true
related:
- samples/dependency-injection/aspnetcore
redirects:
- samples/azure/self-host
- samples/azure/shared-host
---

This sample is compatible with Azure WebJobs SDK 3.0.

## Running in development mode

 1. Start the [Azurite Storage Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite).
 2. Run the solution.

## Code walk-through

This sample contains one project:

- Receiver: A self-hosted endpoint running in a continuous WebJob.

### Receiver

The receiver uses the self-hosting capability to start an endpoint inside a continuously running WebJob.

The `UseNServiceBus` method of [`NServiceBus.Extensions.Hosting`](/nservicebus/hosting/extensions-hosting.md) is used to configure and start the endpoint:

snippet: WebJobHost_Start

> [!NOTE]
> If dependencies need to be shared between the service collection and NServiceBus infrastructure (e.g., message handlers), refer to the [ASP.NET Core sample](/samples/dependency-injection/#related-samples).

A [critical error](/nservicebus/hosting/critical-errors.md) action must be defined to restart the host when a critical error occurs:

snippet: WebJobHost_CriticalError

When the WebJob host stops, the endpoint is automatically stopped with it by the hosting extension.
