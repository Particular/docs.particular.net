---
title: Metrics Upgrade Version 1 to 2
reviewed: 2021-06-05
component: metrics
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


include: metrics-registerobservers


## Changes in RegisterObservers

The [RegisterObservers API](/monitoring/metrics/raw.md#reporting-metrics-data-to-any-external-storage) has changed to take `ref` events.

```csharp
// For Metrics version 2.x
var metrics = endpointConfiguration.EnableMetrics();
metrics.RegisterObservers(
    register: context =>
    {
        foreach (var duration in context.Durations)
        {
            duration.Register(
                observer: (ref DurationEvent @event) =>
                {
                    log.Info($"Duration: '{duration.Name}'. Value: '{@event.Duration}'");
                });
        }
        foreach (var signal in context.Signals)
        {
            signal.Register(
                observer: (ref SignalEvent @event) =>
                {
                    log.Info($"Signal: '{signal.Name}'. Type: '{@event.MessageType}'");
                });
        }
    });

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
                    log.Info($"Duration: '{duration.Name}'. Value: '{length}'");
                });
        }
        foreach (var signal in context.Signals)
        {
            signal.Register(
                observer: () =>
                {
                    log.Info($"signal: '{signal.Name}'");
                });
        }
    });
```
