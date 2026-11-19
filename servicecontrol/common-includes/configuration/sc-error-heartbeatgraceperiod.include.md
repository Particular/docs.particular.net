### ServiceControl/HeartbeatGracePeriod

The period that defines whether an endpoint is considered alive or not since the last received heartbeat.

Type: timespan

Default: `00:00:40` (40 secs)

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_HEARTBEATGRACEPERIOD` |
| **App config key** | `ServiceControl/HeartbeatGracePeriod` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| timespan | `00:00:40` (40 secs) |

When configuring the heartbeat grace period, make sure it is greater than the [heartbeat interval defined by the plugin](/monitoring/heartbeats/install-plugin.md).

> [!NOTE]
> When monitoring multiple endpoints, ensure that the heartbeat grace period is larger than any individual heartbeat interval set by the endpoints.
