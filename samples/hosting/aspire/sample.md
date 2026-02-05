---
title: Hosting endpoints in .NET Aspire
summary: Hosting multiple NServiceBus endpoints in an .NET Aspire application host
component: Core
reviewed: 2024-09-20
---

[.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/) is a stack for developing distributed applications provided by Microsoft.

This sample shows a .NET Aspire AppHost project that orchestrates multiple NServiceBus endpoints, wiring up the required infrastructure pieces including a RabbitMQ broker and PostgreSQL database.

## Running the sample

1. Run the AspireDemo.AppHost project
2. Open the .NET Aspire dashboard
3. Review the metrics, traces, and structured log entries of each of the resources

> [!NOTE]
> This sample requires [Docker](https://www.docker.com/) to run. Ensure the predefined container ports are free and available.

## Code walkthrough

### AspireDemo.AppHost

The [.NET Aspire orchestration project](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/app-host-overview) defines multiple resources and the relationships between them:

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

The [.NET Aspire service defaults](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/service-defaults) project provides extension methods to configure application hosts in a standardized way. This project is referenced by all of the NServiceBus endpoint projects.

The OpenTelemetry configuration has been updated to include NServiceBus metrics and traces.

snippet: add-nsb-otel

### Endpoint projects

Each of the endpoint projects contain the same code to create an application host, apply the configuration from the ServiceDefaults project on the NServiceBus endpoint.

snippet: always-config

Each endpoint project retrieves the connection string for the RabbitMQ broker and configures NServiceBus to use it as a transport:

snippet: transport-config

The Shipping endpoint additionally retrieves the connection string for the PostgreSQL database and configures NServiceBus to use it as a persistence:

snippet: persistence-config

Finally, each endpoint enables NServiceBus installers. Every time the application host is run, the transport and persistence database are recreated and will not contain the queues and tables needed for the endpoints to run. Enabling installers allows NServiceBus to set up the assets that it needs at runtime.

snippet: enable-installers

If you're missing certain capabilities to use .NET Aspire with NServiceBus, [share them and help shape the future of the platform](/shape-the-future/aspire.md).