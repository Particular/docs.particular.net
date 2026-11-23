### ServiceControl/EventRetentionPeriod

The grace period to keep event logs before they are deleted.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_EVENTRETENTIONPERIOD` |
| **App config key** | `ServiceControl/EventRetentionPeriod` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| timespan | 14 days |

Valid range for this setting is from 1 hour to 200 days.
