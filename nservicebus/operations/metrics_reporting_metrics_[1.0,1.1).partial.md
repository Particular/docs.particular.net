## Reporting metrics data

Metrics can be reported to a number of different locations. Each location is updated on a separate interval. 

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

### To Windows Performance Counters

Some of the data captured by the NServiceBus.Metrics component can be forwarded to Windows Performance Counters. See [Performance Counters](./performance-counters.md) for more information.
