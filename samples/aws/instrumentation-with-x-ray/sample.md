---
title: Instrumenting with AWS X-Ray
summary: A sample demonstrating exporting telemetry to the OpenTelemetry Collector, and from there to AWS X-Ray and CloudWatch
reviewed: 2024-11-02
component: SQSLambda
related:
 - nservicebus/sagas
 - samples/aws/dynamodb-simple
 - samples/aws/lambda-sqs
---

INFO: This sample was originally used as part of the talk ["Message processing failed... But what is the root cause?"](https://www.youtube.com/watch?v=Mai-x8rMbLc) by Laila Bougria.

# Sample

This sample showcases the use case presented during the session, a small online retail store. The sample uses [NServiceBus](https://docs.particular.net/) on top of [Amazon SQS](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/welcome.html) and [Amazon SNS](https://docs.aws.amazon.com/sns/latest/dg/welcome.html) to send/publish messages/events between components. The sample makes use of the [Microsoft Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host).

In this sample, we're exporting telemetry to the OpenTelemetry Collector, and from there to [AWS X-Ray](https://docs.aws.amazon.com/xray/latest/devguide/aws-xray.html) and [CloudWatch](https://docs.aws.amazon.com/cloudwatch/). Currently, there's no direct exporter package available to export telemetry directly to AWS X-Ray. Therefore, usage of the OpenTelemetry Collector is required if you're using AWS X-Ray.

The sample demonstrates how to:

- Collect telemetry information from ASP.NET Core, the AWS SDK, the NServiceBus framework
- Define and use an ActivitySource to emit tracing information (in the Stock project)
- Connect traces and logs
- Use the AWS X-Ray extension that transforms the trace-id to an AWS X-Ray-compatible ID
- Set up an OpenTelemetry Collector using the [AWS Distro for OpenTelemetry Collector](https://aws-otel.github.io/docs/getting-started/collector)
- Forward telemetry from components to the [OpenTelemetry Collector](https://opentelemetry.io/docs/collector/) using the [OpenTelemetry Protocol Exporter](https://opentelemetry.io/docs/specs/otel/protocol/exporter/)
- Forward telemetry from the collector to [AWS X-Ray](https://docs.aws.amazon.com/xray/latest/devguide/aws-xray.html) and [CloudWatch](https://docs.aws.amazon.com/cloudwatch/)

## Setting up OpenTelemetry

In the sample, I'm making use of the Microsoft.Extensions.Hosting package. By pulling in the [`OpenTelemetry.Extensions.Hosting`-package](https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting), a method becomes available to add OpenTelemetry to the component.
Both tracing and metrics are configured:

``` c#
services.AddOpenTelemetry()
        .ConfigureResource(resourceBuilder => resourceBuilder.AddService("component-name"))
        .WithTracing(tracingBuilder => tracingBuilder
                                      .AddXRayTraceId()
                                      .AddSource("NServiceBus.Core")
                                      .AddAWSInstrumentation()
                                      .AddOtlpExporter(options =>
                                      {
                                          options.Endpoint = otlpExporterEndpoint;
                                      })
        .WithMetrics(metricsBuilder => metricsBuilder
                                      .AddMeter("NServiceBus.Core")
                                      .AddOtlpExporter(options =>
                                      {
                                          options.Endpoint = otlpExporterEndpoint;
                                      }));
```

The `component-name`-placeholder should reflect the name of the component, as this will be visible in the exported information.

Relevant sources from which to collect traces and metrics should be configured. This sample collects tracing from ASP.NET Core, NServiceBus and the AWS SDK, and metrics for ASP.NET Core and NServiceBus.
OpenTelemetry support is [available in NServiceBus](https://docs.particular.net/nservicebus/operations/opentelemetry?version=core_8) starting from v8. For OpenTelemetry support in NServiceBus v7, there's a [community-supported package](https://github.com/jbogard/NServiceBus.Extensions.Diagnostics) available maintained by Jimmy Bogard.

To collect telemetry information from NServiceBus v8, OpenTelemetry needs to be enabled on the endpoint configuration.

``` c#
endpointConfiguration.EnableOpenTelemetry();
```

This sample uses the [AWS Distro for OpenTelemetry Collector](https://aws-otel.github.io/docs/getting-started/collector).
To start the collector, set the `AWS_ACCESS_KEY_ID`and `AWS_SECRET_ACCESS_KEY` properties in the `docker-compose.yml`-file and then run it.

To export telemetry information to the OpenTelemetry collector, the sample makes use of the [OpenTelemetry Protocol Exporter](https://opentelemetry.io/docs/specs/otel/protocol/exporter/), specifying the connection information to access the collector:

``` c#
  .AddOtlpExporter(options =>
  {
      options.Endpoint = otlpExporterEndpoint;
  }));
```

The OpenTelemetry Collector is set up to export telemetry information to [AWS X-Ray](https://docs.aws.amazon.com/xray/latest/devguide/aws-xray.html) and [CloudWatch](https://docs.aws.amazon.com/cloudwatch/), as shown in the `collector-config.yml`-file in the exporters.

AWS X-Ray uses a specific format for the trace-ids and [doesn't currently support](https://aws-otel.github.io/docs/getting-started/dotnet-sdk/trace-manual-instr#installation) the [W3C TraceContext trace-id standard](https://www.w3.org/TR/trace-context/#traceparent-header). To ensure AWS X-Ray can recognise the trace-id, we need to make use of the `OpenTelemetry.Contrib.Extensions.AWSXRay`-package. This package exposes an extension method called `AddXRayTraceId` that overrides the generated trace-id and replaces it with an X-Ray-compatible trace-id.

**Note** that this affects all traces, independent from the locations to which you're exporting the traces.

## Emitting trace information

As shown in the Stock component, in order to add custom tracing to an application, first, an `ActivitySource` needs to be defined.

``` c#
private static readonly ActivitySource source = new("Stock", "1.0.0");
````

When handling the message, information can be traced as follows:

``` c#
public Task Handle(UpdateProductStock message, IMessageHandlerContext context)
{
    using Activity? activity = source.StartActivity("Stock_UpdateProductStock");

    try 
    {
        var product = ProductStore.Products.Single(x => x.ProductId == message.ProductId);

        activity?.SetTag("ProductId", product.ProductId);
        activity?.AddEvent(new ActivityEvent("Stock_Recalculation_Starting"));

        // update stock

        activity?.AddEvent(new ActivityEvent("Stock_Recalculation_Completed"));
        return Task.CompletedTask;
    }
    catch (Exception e)
    {
        activity?.SetTag("otel.status_code", "ERROR");
        activity?.SetTag("otel.status_description", e.Message);
        throw;
    }
}
```

The usage of the `using`-keyword ensures that the activity is stopped automatically.
Any exceptions are caught to set specific tags on the activity. These tags are propagated to the exporter, and any failures are marked as failed traces in most exporter tools.

## Connecting traces and logs

By connecting the traces and logs, each log message will have a reference to a trace id when one exists. This allows users to easily switch back and forth between log telemetry and trace telemetry.

To connect traces and logs, logging needs to be configured with OpenTelemetry. When using the Microsoft.Extensions.Logging framework, OpenTelemetry can be enabled as part of the logging configuration:

``` c#
services.AddLogging(loggingBuilder =>
       loggingBuilder.AddOpenTelemetry(otelLoggerOptions =>
       {
           otelLoggerOptions.IncludeFormattedMessage = true;
           otelLoggerOptions.IncludeScopes = true;
           otelLoggerOptions.ParseStateValues = true;
           otelLoggerOptions.AddOtlpExporter(options =>
               options.Endpoint = otlpExporterEndpoint
           );
           otelLoggerOptions.AddConsoleExporter();
       }).AddConsole()
   );
```

The OTLP Exporter is set up for logging and also emits logs to the OpenTelemetry Collector.
