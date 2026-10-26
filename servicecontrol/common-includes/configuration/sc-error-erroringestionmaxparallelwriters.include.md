#if-version [7,)
### ServiceControl/ErrorIngestionMaxParallelWriters

The maximum number of batches that are written to storage in parallel. Concurrent writers are supported only with SQL Server and PostgreSQL storage. RavenDB storage always uses `1` writer and logs a warning if a value greater than `1` is explicitly configured.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_ERRORINGESTIONMAXPARALLELWRITERS` |
| **App config key** | `ServiceControl/ErrorIngestionMaxParallelWriters` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `2` with SQL Server or PostgreSQL storage<br/>`1` with RavenDB storage |

The allowed range is `1` to `16`. More writers increase CPU, memory, and storage load; increase the value only if there are no verified bottlenecks.

Custom `IEnrichImportedErrorMessages` implementations must be thread-safe, because they can be invoked concurrently when more than one writer is used.
#end-if
