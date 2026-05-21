## Writing diagnostics to the log

Starting in version 10.2, diagnostics can also be written to the application log. This is useful in environments where writing to the file system is unavailable or impractical, such as containerized or serverless deployments.

snippet: WriteDiagnosticsToLog

When `WriteDiagnosticsToLog` is enabled, diagnostics are written to the log **in addition to** the default file-based output or any custom diagnostics writer. The `AssemblyScanning` section is automatically compacted to prevent exceeding log size limits, such as the [Application Insights 64 KB telemetry limit](https://learn.microsoft.com/en-us/azure/azure-monitor/fundamentals/service-limits#application-insights). If the compacted diagnostics still exceed a safe threshold, the output is truncated and a warning is logged.