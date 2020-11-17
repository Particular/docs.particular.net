#### DegreeOfReceiveParallelism

The number of parallel receive operations that the transport is issuing against the storage queue to pull messages out of it.

Versions 8.1.3 and higher of the transport (or versions 7.5.6 and higher for the 7.x version of the transport) calculate the value at start-up based on the endpoints [message processing concurrency limit](/nservicebus/operations/tuning.md), using the following formula:

|`MaxConcurrency` | `DegreeOfReceiveParallelism` | `Batch Size` |
| :-: | :-:| :-:
| 1 | 1 | 1 |
| 2 | 1 | 2 |
| 3 | 1 | 3 |
| ... | 1 | ... |
| 10 | 1 | 10 |
| ... | 1 | ... |
| 20 | 1 | 20 |
| ... | 1 | ... |
| 32 | 1 | 32 |
| 50 | 2 | 32 / 18 |
| 100 | 4 | 32 / 32 / 32 / 4 |
| 200 | 7 | 32 / 32 / 32 / 32 / 32 / 32 / 32 / 8 |
| 1000 | 31 | 31 x 32 / 8 |

When the `DegreeOfReceiveParallelism` is explicitly set, the batch size is dynamically adjusted to fulfill the endpoints message processing limits if possible up to the allowed maximum of 32. For example, with a maximum concurrency set to `100` and a `DegreeOfReceiveParallelism` fixed to `3` an underfetching of `4` messages might occur. Therefore it is advised to tweak both `DegreeOfReceiveParallelism` to be in alignment with the `BatchSize` if required. In most cases, the dynamic calculation should be sufficient and no adjustment is required.

Versions 7.x to 7.5.5, and 8.x to 8.1.2, dynamically calculate the value based on the endpoints [message processing concurrency limit](/nservicebus/operations/tuning.md), using the following equation:

```
Degree of parallelism = square root of MaxConcurrency
```

|`MaxConcurrency` | `DegreeOfReceiveParallelism` |
| :-: |:-:|
| 1 | 1 |
| 10 | 3 |
| 20 | 4 |
| 50 | 7 |
| 100 | 10 |
| 200 | 14 |
| 1000 | 32 [max] |

This means that `DegreeOfReceiveParallelism` message processing loops will receive up to the configured `BatchSize` number of messages in parallel. For example, using a `BatchSize` of 32 (the default) and parallelism set to 10 will allow the transport to receive 320 messages from the storage queue at the same time.

WARNING: Changing the value of `DegreeOfReceiveParallelism` will influence the total number of storage operations against Azure Storage Services and can result in higher costs.

WARNING: The values of `BatchSize` , `DegreeOfParallelism`, `Concurrency`, [ServicePointManager Settings](/persistence/azure-table/performance-tuning.md) and the other parameters like `MaximumWaitTimeWhenIdle` must be selected carefully to get the desired speed from the transport without exceeding [the boundaries](https://docs.microsoft.com/en-us/azure/azure-subscription-service-limits) of the allowed number of operations per second.
