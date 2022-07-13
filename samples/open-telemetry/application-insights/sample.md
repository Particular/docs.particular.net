---
title: Exporting OpenTelemetry data to Application Insights
summary: How to configure an endpoint to export OpenTelemetry traces to Application Insights
reviewed: 2022-07-13
component: Core
related:
---

## Introduction

This sample shows how to capture NServiceBus OpenTelemetry traces and export them to Application Insights in Azure.

## Prerequisites

This sample requires an Application Insights connection string.

## Running the sample

1. Create an Application Insights resource in Azure
2. Copy the connection string from the Azure portal dashboard into the sample
3. Start the sample endpoint
4. Press any key to send a message, or ESC to quit
5. On the Aure portal dashboard, open the Monitoring -> Logs panel
6. Run the query `requests` to see a list of the requests. Review the `customDimensions` attribute to see NServiceBus headers

## Code walk-through

### Enable tracing

The endpoint configures an OpenTelemetry trace provider that includes the `NServiceBus.Core` source and exports traces to Azure Monitor.

snippet: enable-tracing

### Enable meters

The endpoint also configures an OpenTelemetry meter provider that includes the `NServiceBus.Core` meter and exports data to Azure Monitor.

snippet: enable-meters

#### Meter shim

The Azure Monitor exporter package does not currently support exporting meter data so the sample includes custom code to collect the meter.

The custom meter exporter captures all meters that begin with `nservicebus.` of the `long` sum type, and forwards them to a `TelemetryClient`.

snippet: custom-meter-exporter

This exporter is installed into the meter provider builder with a custom extension method. The exporter is wrapped by a `PeriodicExportingMetricReader` instance that executes the exporter once every 10 seconds.

snippet: custom-meter-exporter-installation