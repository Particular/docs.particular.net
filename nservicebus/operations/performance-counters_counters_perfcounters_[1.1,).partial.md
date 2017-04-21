## Counters

All counters are defined in the NuGet package dependency [NServiceBus.Metrics](https://www.nuget.org/packages/NServiceBus.Metrics/). The dependency is automatically pulled in.

For more information about the metrics defined consult the [Metrics](metrics.md) documentation page.

### Update counters

The counters are periodically updated every two seconds per default. To override the update interval use:

snippet: update-counters-every