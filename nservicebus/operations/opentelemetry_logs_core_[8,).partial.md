## Logging

NServiceBus supports logging out of the box. To collect OpenTelemetry-compatible logging in NServiceBus endpoints, it's possible to configure the endpoint to connect traces and logging when using `Microsoft.Extensions.Logging` package. See the [_Connecting OpenTelemetry traces and logs_ sample](/samples/open-telemetry/logging) for more details.

### Recoverability structured log properties

Recoverability action log entries are emitted via `Microsoft.Extensions.Logging` and include named structured properties. When using a structured logging backend such as Serilog, Seq, or Application Insights, these properties are captured as queryable key-value pairs rather than text embedded in the message string:

| Recoverability action | Structured properties |
|---|---|
| Immediate retry | `MessageId` |
| Delayed retry | `MessageId`, `Delay` |
| Move to error queue | `MessageId`, `ErrorQueue` |
| Discard | `MessageId`, `Reason` |

For example, all messages moved to a specific error queue can be queried by filtering on `ErrorQueue`, or all delayed retries for a specific message can be found by filtering on `MessageId`, without needing to parse log message strings.
