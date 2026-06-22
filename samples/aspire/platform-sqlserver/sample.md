---
title: Particular Platform with SQL Server in Aspire
summary: Orchestrating the Particular Platform with SQL Server transport via Aspire
component: AspireHostPlatform
reviewed: 2026-05-22
---

[Aspire](https://aspire.dev/) is a stack for developing distributed applications provided by Microsoft.

This sample shows an Aspire AppHost project that orchestrates the Particular Platform, multiple NServiceBus endpoints, wiring up the required infrastructure pieces when using the [SQL Server](/transports/sql/) transport.

If you're missing certain capabilities to use Aspire with NServiceBus, [share them and help shape the future of the platform](/shape-the-future/aspire.md).

## Running the sample

1. Run the AspireDemo.AppHost project
2. Open the Aspire dashboard
3. Review the metrics, traces, and structured log entries of each of the resources

> [!NOTE]
> This sample requires [Docker](https://www.docker.com/) to run. Ensure the predefined container ports are free and available.
> It also assumes a SQL Server instance has been setup.

## Code walkthrough

### AspireDemo.AppHost

The [Aspire orchestration project](https://aspire.dev/get-started/app-host/?lang=csharp) defines multiple resources and the relationships between them:

- A SQL Server connection named `transport`
- Two projects, each of which is an NServiceBus endpoint. All of these projects reference the `transport` resource.
  - `clientui`
  - `sales`
- ServiceControl error, audit and monitoring instances
- ServicePulse

#### Platform configuration

`AddParticularPlatform` registers the Particular Platform as a resource named `particular`. The `WithTransportSqlServer` extension points the platform at the `transport` connection string resource defined earlier, so that the ServiceControl instances connect to the same SQL Server database as the endpoints.

snippet: platform-config

#### Transport

This sample assumes that a SQL Server instance has already been provisioned. It is referenced through a connection string resource named `transport`, which is then passed to `WithTransportSqlServer`:

snippet: transport

Alternatively, the broker can be defined as a [SQL Server resource](https://aspire.dev/integrations/databases/sql-server/sql-server-host/) managed by Aspire (from the `Aspire.Hosting.Azure.ServiceBus` package), which Aspire provisions as a real database. Because `WithTransportSqlServer` accepts any resource that exposes a connection string, the resulting resource can be passed in directly:

```csharp
var transport = builder.AddSqlServer("transport");

builder
    .AddParticularPlatform("particular")
    .WithTransportSqlServer(transport);
```

#### ServiceControl Database

The platform requires a database to store the data managed by its ServiceControl instances. `AddPersistenceRavenDb` adds a [RavenDB](/persistence/ravendb/) resource named `particular-persistence` for this purpose.

snippet: persistence

#### Default components

`AddDefaultComponents` registers the remaining platform components using their default configuration — the ServiceControl audit and monitoring instances and ServicePulse. The error instance is added explicitly above so that usage reporting can be configured on it.

snippet: default-components

#### Endpoints

Each NServiceBus endpoint is added as an Aspire project and linked to the platform with `WithParticularPlatform`. This wires the endpoint to the platform's transport connection string. The `ClientUI` endpoint additionally uses `WaitFor(sales)` so that the `Sales` endpoint exists before it starts sending messages to it.

snippet: endpoints

### AspireDemo.ServiceDefaults

The [Aspire service defaults](https://aspire.dev/get-started/csharp-service-defaults/) project provides extension methods to configure application hosts and NServiceBus endpoints in a standardized way. This project is referenced by all of the NServiceBus endpoint projects.

The OpenTelemetry configuration has been updated to include NServiceBus metrics and traces.

snippet: add-nsb-otel

Each endpoint project retrieves the connection string for the Azure ServiceBus broker and configures NServiceBus to use it as a transport:

snippet: transport-config

Finally, each endpoint enables NServiceBus installers. Every time the application host is run, the transport and ServiceControl database are recreated and will not contain the queues and tables needed for the endpoints to run. Enabling installers allows NServiceBus to set up the assets that it needs at runtime.

snippet: enable-installers

### Endpoint projects

Each of the endpoint projects contain the same code to create an application host, apply the configuration from the ServiceDefaults project on the NServiceBus endpoint.

snippet: endpoint-config

To demonstrate the platform's error handling, the `Sales` endpoint's handler throws an exception for a random subset of the messages it receives:

snippet: random-error

Failed messages are moved to the error queue, where they can be inspected and retried from ServicePulse.

