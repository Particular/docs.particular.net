### Polling interval

Messages are checked for expiration every second. The polling interval can be configured using:

snippet: DelayedDeliveryProcessingInterval

### Polling batch size

On each query, a batch of messages is picked and dispatched. The maximal size of the batch can be specified with:

snippet: DelayedDeliveryBatchSize
