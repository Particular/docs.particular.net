### ServiceControl.Audit/QueryTimeoutInSeconds

Configures the maximum duration, in seconds, that an audit message query (for example, a message search or a conversation lookup issued by ServicePulse) is allowed to run before it is cancelled. This protects the RavenDB server from queries over very large data sets that would otherwise run for a long time and consume large amounts of temporary disk space. Values larger than one hour fall back to the default.

A query that runs out of its allowed time is answered with HTTP status `504 Gateway Timeout` and a problem details body that names this setting. The error instance treats that answer as a missing instance rather than as an instance with no data. The maximum duration the error instance will wait for an audit instance is bounded by its own [`ServiceControl/QueryTimeoutInSeconds`](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrol-querytimeoutinseconds), independently of this value.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_QUERYTIMEOUTINSECONDS` |
| **App config key** | `ServiceControl.Audit/QueryTimeoutInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `60` (1 minute) |
