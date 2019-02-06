Starting from version 7.5.5 of the transport the batch size is dynamically calculated based on the endpoints [message processing concurrency limit] (/nservicebus/operations/tuning.md) unless explicitely specified. The batch size is calculated based on the following formula

|`MaxConcurrency` | `Batch Size` |
| :-: |:-:|
| 1 | 1 |
| 2 | 2 |
| 3 | 3 |
| ... | ... |
| 32 | 32  [max] |

If the message processing concurrency limit is higher than the maximum batch size the [degree of parallelism](/transports/azure-storage-queues/configuration.md#configuration-parameters-degreeofreceiveparallelism) is dynamically increased, unless explicitely specified, to fullfil the concurrency needs of the endpoint while avoiding overfetching of messages. This is done to decrease the likelyhood of message visibility timeouts.

Versions starting from 7.x to 7.5.4 default to a batch size of 32.