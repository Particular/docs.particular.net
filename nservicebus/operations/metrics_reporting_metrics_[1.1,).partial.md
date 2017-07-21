## Reporting metrics data

Metrics can be reported in a few different ways.

### To Windows Performance Counters

Some of the data captured by the NServiceBus.Metrics component can be forwarded to Windows Performance Counters. See [Performance Counters](./performance-counters.md) for more information.

### To any external storage

Custom observers might be registered to access every value reported by probes.

snippet: Metrics-Observers