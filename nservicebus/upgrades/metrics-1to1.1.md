---
title: Metrics Upgrade Version 1.0 to 1.1
reviewed: 2021-06-05
component: metrics
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


include: metrics-registerobservers


### EnableMetricTracing

Replace with explicit calls to [Trace.WriteLine](https://msdn.microsoft.com/en-us/library/system.diagnostics.trace.writeline.aspx).

```csharp
// For Metrics version 1.x
var metrics = endpointConfiguration.EnableMetrics();
metrics.RegisterObservers(
    register: context =>
    {
        foreach (var duration in context.Durations)
        {
            duration.Register(
                observer: length =>
                {
                    Trace.WriteLine($"Duration '{duration.Name}'. Value: '{length}'");
                });
        }
        foreach (var signal in context.Signals)
        {
            signal.Register(
                observer: () =>
                {
                    Trace.WriteLine($"Signal: '{signal.Name}'");
                });
        }
    });

// For Metrics version 1.x
var metrics = endpointConfiguration.EnableMetrics();
metrics.EnableMetricTracing(TimeSpan.FromSeconds(5));
```


### EnableLogTracing

Replace with explicit calls to an [NServiceBus logger](/nservicebus/logging/#writing-a-log-entry).

```csharp
// For Metrics version 1.x
//TODO: the logger instance should be a static field
var log = LogManager.GetLogger("LoggerName");

var metrics = endpointConfiguration.EnableMetrics();
metrics.RegisterObservers(
    register: context =>
    {
        foreach (var duration in context.Durations)
        {
            duration.Register(
                observer: length =>
                {
                    log.Info($"Duration: '{duration.Name}'. Value: '{length}'");
                });
        }
        foreach (var signal in context.Signals)
        {
            signal.Register(
                observer: () =>
                {
                    log.Info($"Signal: '{signal.Name}'");
                });
        }
    });

// For Metrics version 1.x
var metrics = endpointConfiguration.EnableMetrics();
metrics.EnableLogTracing(TimeSpan.FromSeconds(5), LogLevel.Info);
```


### EnableCustomReport

Replace with explicit calls to the custom method.

```csharp
// For Metrics version 1.x
var metrics = endpointConfiguration.EnableMetrics();
metrics.RegisterObservers(
    register: context =>
    {
        foreach (var duration in context.Durations)
        {
            duration.Register(
                observer: length =>
                {
                    ProcessMetric(duration, length);
                });
        }
        foreach (var signal in context.Signals)
        {
            signal.Register(
                observer: () =>
                {
                    ProcessMetric(signal);
                });
        }
    });

// For Metrics version 1.x
var metrics = endpointConfiguration.EnableMetrics();
metrics.EnableCustomReport(
    func: data => ProcessMetric(data),
    interval: TimeSpan.FromSeconds(5));
```
