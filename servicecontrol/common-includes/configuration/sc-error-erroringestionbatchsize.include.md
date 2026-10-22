#if-version [7,)
### ServiceControl/ErrorIngestionBatchSize

The maximum number of messages in each ingestion batch. If not set, the default is [`MaximumConcurrencyLevel`](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolmaximumconcurrencylevel) divided by the resolved [number of writers](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolerroringestionmaxparallelwriters).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_ERRORINGESTIONBATCHSIZE` |
| **App config key** | `ServiceControl/ErrorIngestionBatchSize` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | [`MaximumConcurrencyLevel`](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolmaximumconcurrencylevel) divided by the resolved [number of writers](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolerroringestionmaxparallelwriters) |

The allowed range is `1` to `1000`. A value that is set explicitly is used as configured. A batch larger than `MaximumConcurrencyLevel` can never fill because messages occupy receive slots until their batch is stored. Larger batches use more memory.
#end-if
