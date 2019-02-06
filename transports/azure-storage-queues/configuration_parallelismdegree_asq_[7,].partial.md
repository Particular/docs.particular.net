#### DegreeOfReceiveParallelism

The number of parallel receive operations that the transport is issuing against the storage queue to pull messages out of it.

The value is dynamically calculated based on the endpoints [message processing concurrency limit](/nservicebus/operations/tuning.md), using the following equation:

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

This means that `DegreeOfReceiveParallelism` message processing loops will receive up to the configured `BatchSize` number of messages in parallel. For example using a `BatchSize` of 32 (Default) and parallelism set to 10 will allow the transport to receive 320 messages from the storage queue at the same time.

WARNING: Changing the value of `DegreeOfReceiveParallelism` will influence the total number of storage operations against Azure Storage Services and can result in higher costs.

WARNING: The values of `BatchSize` , `DegreeOfParallelism`, `Concurrency`, [ServicePointManager Settings](/persistence/azure-storage/performance-tuning.md) and the other parameters like `MaximumWaitTimeWhenIdle` have to be selected carefully in order to get the desired speed out of the transport while not exceeding [the boundaries](https://docs.microsoft.com/en-us/azure/azure-subscription-service-limits) of the allowed number of operations per second.
