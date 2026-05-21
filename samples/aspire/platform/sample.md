---
title: Particular Platform in Aspire
summary: Hosting the necessary components of the Particular Service Platform in an Aspire AppHost
component: Core
reviewed: 2026-05-07
---

[Aspire](https://aspire.dev/) is a stack for developing distributed applications provided by Microsoft.

This sample shows an Aspire AppHost project that orchestrates multiple NServiceBus endpoints, making use of the learning transport.

## Running the sample

1. Run the AspireDemo.AppHost project
2. Open the Aspire dashboard
3. Review the metrics, traces, and structured log entries of each of the resources

> [!NOTE]
> This sample requires [Docker](https://www.docker.com/) to run. Ensure the predefined container ports are free and available.

## Code walkthrough

### AspireDemo.AppHost

The [Aspire orchestration project](https://aspire.dev/get-started/app-host/?lang=csharp) defines multiple resources and the relationships between them:

- A Particular platform resource named `particular`, which includes:
  - Default ServiceControl error, audit and monitoring instances
  - ServicePulse
  - Learning transport
- Two projects, each of which is an NServiceBus endpoint. All of these projects reference the `particular` resource to access the transport connection string.
  - `billing`
  - `sales`

snippet: app-host

### AspireDemo.ServiceDefaults

The [Aspire service defaults](https://aspire.dev/get-started/csharp-service-defaults/) project provides extension methods to configure application hosts in a standardized way. This project is referenced by all of the NServiceBus endpoint projects.

The OpenTelemetry configuration has been updated to include NServiceBus metrics and traces.

snippet: add-nsb-otel

The endpoints are configured to use the learning transport, by using the path provided by the `particular` resource:

snippet: transport-config

Additionally, the shared config enables NServiceBus installers. Every time the application host is run, the transport and persistence database are recreated and will not contain the queues and tables needed for the endpoints to run. Enabling installers allows NServiceBus to set up the assets that it needs at runtime.

snippet: enable-installers

### Endpoint projects

Each of the endpoint projects contain the same code to create an application host to apply the configuration from the ServiceDefaults project on the NServiceBus endpoint.

snippet: endpoint-config

If you're missing certain capabilities to use Aspire with NServiceBus, [share them and help shape the future of the platform](/shape-the-future/aspire.md).