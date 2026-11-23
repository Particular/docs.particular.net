### ServiceControl/TimeToRestartErrorIngestionAfterFailure

Controls the maximum time delay to wait before restarting the error ingestion pipeline after detecting a connection problem.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_TIMETORESTARTAUDITINGESTIONAFTERFAILURE` |
| **App config key** | `ServiceControl/TimeToRestartAuditIngestionAfterFailure` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| timespan | 60 seconds |

Valid settings are between 5 seconds and 1 hour.
