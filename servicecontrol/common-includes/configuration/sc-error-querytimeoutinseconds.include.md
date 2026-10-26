#if-version [6.20,)
### ServiceControl/QueryTimeoutInSeconds

Configures the maximum duration, in seconds, that a failed message view query (for example, a message search or a conversation lookup issued by ServicePulse) is allowed to run before it is cancelled. This protects the database server from queries over very large data sets that would otherwise run for a long time and consume large amounts of temporary disk space. Values larger than one hour fall back to the default. Applies to all persisters. On the SQL Server and PostgreSQL persisters, the database commands issued by these queries use this value as their command timeout, so `Database/CommandTimeout` does not apply to them.

A query that runs out of its allowed time is answered with HTTP status `504 Gateway Timeout` and a problem details body that names this setting.

When the instance gathers a message view from its own database and the configured [audit instances](/servicecontrol/audit-instances/), a timed-out or unreachable instance does not fail the request. The response contains the data of the instances that did answer, carries no `ETag`, and lists the missing instances in the `X-Particular-Incomplete-Results` header as `instanceId:reason` entries, where the reason is `timeout`, `unavailable` or `error`. Only when no instance answered, and at least one of them timed out, is the request answered with `504 Gateway Timeout`.

The instance waits for an audit instance's answer for at most this duration, so it also bounds the whole query when an audit instance is slow, unresponsive, or configured with a larger `ServiceControl.Audit/QueryTimeoutInSeconds`. An audit instance that has not answered in time is reported as missing; its own limit still ends the query on its side.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_QUERYTIMEOUTINSECONDS` |
| **App config key** | `ServiceControl/QueryTimeoutInSeconds` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `60` (1 minute) |
#end-if
