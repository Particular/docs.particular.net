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

> [!WARNING]
> Custom `IEnrichImportedAuditMessages` implementations must be thread-safe. They are called concurrently whenever more than one writer is used, which is the default for every storage type.
#end-if
