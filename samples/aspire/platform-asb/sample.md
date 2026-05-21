---
title: Particular Platform with ASB in Aspire
summary: Orchestrating the Particular Platform with Azure ServiceBus transport via Aspire
component: Core
reviewed: 2026-05-21
---

[Aspire](https://aspire.dev/) is a stack for developing distributed applications provided by Microsoft.

This sample shows an Aspire AppHost project that orchestrates the PArticular Platform, multiple NServiceBus endpoints, wiring up the required infrastructure pieces including an ASB transport.

## Running the sample

1. Run the AspireDemo.AppHost project
2. Open the Aspire dashboard
3. Review the metrics, traces, and structured log entries of each of the resources

> [!NOTE]
> This sample requires [Docker](https://www.docker.com/) to run. Ensure the predefined container ports are free and available.

## Code walkthrough

### AspireDemo.AppHost

The [Aspire orchestration project](https://aspire.dev/get-started/app-host/?lang=csharp) defines multiple resources and the relationships between them:

- A RabbitMQ instance named `transport`
- A PostgreSQL server named `database`
  - A database named `shipping-db`
  - An instance of [pgweb](https://sosedoff.github.io/pgweb/) to access the database
- Four projects, each of which is an NServiceBus endpoint. All of these projects reference the `transport` resource.
  - `clientui`
  - `billing`
  - `sales`
  - `shipping` - also has a reference to the `shipping-db` resource
- ServiceControl error, audit and monitoring instances
- ServicePulse

snippet: app-host

### AspireDemo.ServiceDefaults

The [Aspire service defaults](https://aspire.dev/get-started/csharp-service-defaults/) project provides extension methods to configure application hosts in a standardized way. This project is referenced by all of the NServiceBus endpoint projects.

The OpenTelemetry configuration has been updated to include NServiceBus metrics and traces.

snippet: add-nsb-otel

### Endpoint projects
