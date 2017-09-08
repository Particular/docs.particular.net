## Counters

All counters are defined in the NuGet package dependency [NServiceBus.Metrics](https://www.nuget.org/packages/NServiceBus.Metrics/). The dependency is automatically pulled in.

For more information about the metrics defined consult the [Metrics](metrics.md) documentation page.


### Configuration

This counters can be enabled using the the following code:

snippet: enable-criticaltime

In the NServiceBus Host this counters are enabled by default.

Setting up SLA value can be done using following code:

snippet: enable-sla


### Update counters

The counters are periodically updated every two seconds by default. To override the update interval:

snippet: update-counters-every