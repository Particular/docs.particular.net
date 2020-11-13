Starting from version 8.1.3 of the transport (and from version 7.5.6 for the 7.x version of the transport), the batch size is dynamically calculated based on the endpoints [message processing concurrency limit](/nservicebus/operations/tuning.md) unless explicitly specified. The batch size is calculated based on the following formula

|`MaxConcurrency` | `Batch Size` |
| :-: |:-:|
| 1 | 1 |
| 2 | 2 |
| 3 | 3 |
| ... | ... |
| 32 | 32  [max] |

If the message processing concurrency limit is higher than the maximum batch size the [degree of parallelism](/transports/azure-storage-queues/configuration.md#configuration-parameters-degreeofreceiveparallelism) is dynamically increased, unless explicitly specified, to fulfill the concurrency needs of the endpoint while avoiding over fetching of messages. This is done to decrease the likelihood of message visibility timeouts.

Versions 7.x to 7.5.5 and 8.x to 8.1.2 default to a batch size of 32.
