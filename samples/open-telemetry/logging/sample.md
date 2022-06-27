---
title: Connecting OpenTelemetry traces and logs
summary: How to connect OpenTelemetry traces to logs
reviewed: 2022-06-27
component: Extensions.Logging
related:
- nservicebus/logging
---

This sample shows how to configure an NServiceBus endpoint to collect telemetry data and link the traces and logs together.

First, OpenTelemetry needs to be configured on the endpoint:

snippet: otel-config

In this example, all telemetry is being sent to a `ConsoleExporter`, but any OpenTelemetry-compliant exporter will work.

Then, the logs emitted by the application can be connected to the traces as follows:

snippet: otel-logging

This will apply the OpenTelemetry logging format to all the logs. More importantly, it will [correlate all logs](https://opentelemetry.io/docs/reference/specification/logs/overview/#log-correlation) to the active traces.
This means that each log statement will include a `TraceId` and `SpanId` as part of the log entry.