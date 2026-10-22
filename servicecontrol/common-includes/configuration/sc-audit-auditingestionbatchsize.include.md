#if-version [7,)
### ServiceControl.Audit/AuditIngestionBatchSize

The maximum number of messages in each ingestion batch. If not set, the default is [`MaximumConcurrencyLevel`](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditmaximumconcurrencylevel) divided by the resolved [number of writers](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditauditingestionmaxparallelwriters).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUDITINGESTIONBATCHSIZE` |
| **App config key** | `ServiceControl.Audit/AuditIngestionBatchSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | [`MaximumConcurrencyLevel`](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditmaximumconcurrencylevel) divided by the resolved [number of writers](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditauditingestionmaxparallelwriters) |

The allowed range is `1` to `1000`. A value that is set explicitly is used as configured. A batch larger than `MaximumConcurrencyLevel` can never fill because messages occupy receive slots until their batch is stored. Larger batches use more memory.
#end-if
