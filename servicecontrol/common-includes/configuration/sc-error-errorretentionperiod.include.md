### ServiceControl/ErrorRetentionPeriod

The grace period that errored messages are kept before they are deleted.

For a message to be considered for deletion, it needs to have a status of either `Archived`, `RetryIssued`, or `Resolved`.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_ERRORRETENTIONPERIOD` |
| **App config key** | `ServiceControl/ErrorRetentionPeriod` |
| **SCMU field** | `ERROR RETENTION PERIOD` |

| Type | Default value |
| --- | --- |
| timespan | None (required) (SCMU slider defaults to `15 Days`) |

Valid range for this setting is between 5 days and 45 days.
