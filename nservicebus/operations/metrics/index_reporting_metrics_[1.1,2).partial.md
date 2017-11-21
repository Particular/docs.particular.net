## Reporting metrics data

Metrics can be reported in a few different ways.


### To Windows Performance Counters

Some of the data captured by the NServiceBus.Metrics component can be forwarded to Windows Performance Counters. See [Performance Counters](./performance-counters.md) for more information.


### To any external storage

Custom observers might be registered to access every value reported by probes.

snippet: Metrics-Observers

WARNING: Methods provided below that enable logging metrics data are obsoleted in version 1.1 and will be removed in the next major version.


### To NServiceBus log

Metrics data can be written to the [NServiceBus Log](/nservicebus/logging/).

snippet: Metrics-Log

NOTE: By default metrics will be written to the log at the `DEBUG` log level. The API allows this parameter to be customized.

snippet: Metrics-Log-Info


### To trace log

Metrics data can be written to [System.Diagnostics.Trace](https://msdn.microsoft.com/en-us/library/system.diagnostics.trace.aspx).

snippet: Metrics-Tracing


### To custom function

Metrics data can be consumed by a custom function.

snippet: Metrics-Custom-Function
