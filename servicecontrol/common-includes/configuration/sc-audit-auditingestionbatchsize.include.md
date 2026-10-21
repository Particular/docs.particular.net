#if-version [7,)
### ServiceControl.Audit/AuditIngestionBatchSize

The maximum number of messages in each ingestion batch. If not set, the default is `MaximumConcurrencyLevel` divided by the resolved number of writers. With the default of `2` writers, this is `16` for most transports, or `5` for SQL Server and PostgreSQL transports.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUDITINGESTIONBATCHSIZE` |
| **App config key** | `ServiceControl.Audit/AuditIngestionBatchSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `max(1, MaximumConcurrencyLevel / writers)` |

The allowed range is `1` to `1000`. A value that is set explicitly is used as configured. A batch larger than `MaximumConcurrencyLevel` can never fill because messages occupy receive slots until their batch is stored. Larger batches use more memory.
#end-if
