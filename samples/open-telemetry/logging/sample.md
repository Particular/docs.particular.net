---
title: Connecting OpenTelemetry traces and logs
summary: How to connect OpenTelemetry traces to logs in an NServiceBus endpoint
reviewed: 2024-07-26
component: Core
related:
 - nservicebus/operations/opentelemetry
 - nservicebus/logging
---

This sample shows how to configure an NServiceBus endpoint to link OpenTelemetry traces and logs together.

## Configuration

First, configure OpenTelemetry to capture NServiceBus traces:

snippet: otel-config

In this example, all telemetry is sent to a `ConsoleExporter`, but any OpenTelemetry-compliant exporter will work.

Next, connect the logs emitted by the application to the traces as follows:

snippet: otel-logging

This will apply the OpenTelemetry logging format to all the logs. More importantly, it will [correlate all logs](https://opentelemetry.io/docs/reference/specification/logs/overview/#log-correlation) to the active traces.
This means that each log statement will include a `TraceId` and `SpanId` as part of the log entry.

partial: enableotel

## Running the sample

When running the sample, the following log statement from the message handler:

snippet: log-statement

will lead to the following output:

```
 info: MyMessageHandler[0]
 Received message #54
 LogRecord.Timestamp:               2022-06-28T09:02:34.5342602Z
 LogRecord.TraceId:                 964b320925ab08cd2134c02d0abe920d
 LogRecord.SpanId:                  066fce94c5438add
 LogRecord.TraceFlags:              Recorded
 LogRecord.CategoryName:            MyMessageHandler
 LogRecord.LogLevel:                Information
 LogRecord.FormattedMessage:        Received message #54
 LogRecord.StateValues (Key:Value):
 {OriginalFormat}: Received message #54
 LogRecord.ScopeValues (Key:Value):
 [Scope.0]:SpanId: 066fce94c5438add
 [Scope.0]:TraceId: 964b320925ab08cd2134c02d0abe920d
 [Scope.0]:ParentId: e595cc6a4d9b0768
```

The console will also show information for the captured traces:

```
Activity.TraceId:          964b320925ab08cd2134c02d0abe920d
Activity.SpanId:           066fce94c5438add
Activity.TraceFlags:           Recorded
Activity.ParentSpanId:    eb999a0dadc477ea
Activity.ActivitySourceName: NServiceBus.Core
Activity.DisplayName: MyMessageHandler
Activity.Kind:        Internal
Activity.StartTime:   2022-06-28T09:02:34.5322602Z
Activity.Duration:    00:00:00.0017675
Activity.Tags:
    nservicebus.handler.handler_type: MyMessageHandler
StatusCode : Ok
Resource associated with Activity:
    service.name: Samples.Hosting.GenericHost
    service.instance.id: 93d66e53-7537-4e21-99f9-e62b579b5fa3
```
