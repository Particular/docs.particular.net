## RegisterObservers replaces basic methods

The `EnableMetricTracing`, `EnableCustomReport` and `EnableLogTracing` methods have been deprecated and replaced with the more extensible [`RegisterObservers`](/monitoring/metrics/raw.md#reporting-metrics-data-to-any-external-storage).

Also note that the older methods supported an `interval` TimeSpan. It is now the responsibility of the implementing code to batch metrics and flush them as needed.
