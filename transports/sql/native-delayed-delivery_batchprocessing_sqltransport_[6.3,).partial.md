### Batch size

The SQL Server transport processes delayed messages in batches. Each time the transport queries for messages, it will use the batch size setting to limit the number of messages to be processed. This batch size is set to 100 by default, but can be configured using:

snippet: DelayedDeliveryBatchSize
