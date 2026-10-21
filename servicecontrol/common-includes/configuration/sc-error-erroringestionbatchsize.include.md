#if-version [7,)
### ServiceControl/ErrorIngestionBatchSize

The maximum number of messages in each ingestion batch. If not set, the default is `MaximumConcurrencyLevel` divided by the resolved number of writers. With the default concurrency of `10`, this is `5` for SQL Server and PostgreSQL storage, or `10` for RavenDB storage.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_ERRORINGESTIONBATCHSIZE` |
| **App config key** | `ServiceControl/ErrorIngestionBatchSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `max(1, MaximumConcurrencyLevel / writers)` |

The allowed range is `1` to `1000`. A value that is set explicitly is used as configured. A batch larger than `MaximumConcurrencyLevel` can never fill because messages occupy receive slots until their batch is stored. Larger batches use more memory.
#end-if
