#if-version [7,)
### ServiceControl.Audit/AuditIngestionMaxParallelWriters

The maximum number of batches that are written to storage in parallel.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUDITINGESTIONMAXPARALLELWRITERS` |
| **App config key** | `ServiceControl.Audit/AuditIngestionMaxParallelWriters` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `2` |

The allowed range is `1` to `16`. More writers increase CPU, memory, and storage load; increase the value only if there are no verified bottlenecks.
#end-if
