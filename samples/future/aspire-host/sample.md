---
title: Running NServiceBus endpoints in Aspire
summary: How to host NServiceBus endpoints in Aspire
reviewed: 2024-09-19
component: Core
related:
  - nservicebus/operations/opentelemetry
  - samples/open-telemetry
---

This sample illustrates how to run NServiceBus endpoints in [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview).
This example is based on the saga tutorial, which is part of the [NServiceBus learning path](/tutorials/nservicebus-sagas).

## Prerequisites

There aren't any prerequisites, that's the beauty of Aspire: .NET Aspire will take care of wiring up the necessary infrastructure and components.

## Code walk-through

### The AppHost project

The `AppHost` project is where you configure all your dependencies, applications, and any specific infrastructure that are required, like a database.
These are all configured in the `Program.cs`-file.

First, the message broker is configured. The sample uses RabbitMQ as a broker, which is supported through the [RabbitMQ transport](/transports/rabbitmq) for NServiceBus.

snippet: setup-rabbit

Next, required databases are configured. The sample uses PostgresSQL as a database across all endpoints, which is supported by the [SQL Persistence transport](/persistence/sql) in NServiceBus.
In this scenario, only the `Shipping`-endpoint requires a database. However, in a real-world scenario, more endpoints would require their own database, requiring them all to be configured here.

snippet: setup-postgres

Next, the endpoints are configured. The sample consists of four endpoints: ClientUI, Sales, Billing and Shipping.
For each endpoint, a project is added, referencing the transport, and where applicable, the persistence.

snippet: setup-endpoint

Once all the necessary infrastructure, components and dependencies are configured, the `AppHost` is started.

snippet: start-host

### The ServiceDefaults project

The ServiceDefaults projects adds common .NET Aspire services, including service discovery, health checks, and OpenTelemetry.

In this sample, OpenTelemetry is enabled to capture telemetry from endpoints:

snippet: enable-otel

To ensure that NServiceBus emits traces, metrics and logs through OpenTelemetry, OpenTelemetry needs to be enabled on all individual endpoints:

snippet: enable-endpoint-otel

Another service that is added, is a health check.

### The endpoints

The endpoint configurations for the endpoints doesn't change in a .NET Aspire project, they're the same as in any other NServiceBus project.

## Running the sample

To run the sample, open the solution in Visual Studio, and press F5 to start the `AppHost` project. If you prefer using JetBrains Rider, you'll need to use the Aspire plugin.

.NET Aspire will pull down the required docker images and start them, and continue wiring up all the necessary components in the order that was specified in the configuration.

The `ClientUI`-project is set up to immediately start sending out messages, slowing down after each message.
The Aspire dashboard will show all the components that are running, and as the telemetry is collected, it should become visible on the Traces and Logs section of the dashboard.

