
### UseNonTransactionalQueues

WARNING: This setting should be used with caution. As the queues are not transactional, any message that has an exception during processing will not be rolled back to the queue. Therefore this setting must only be used where loss of messages is an acceptable scenario. 

While there may be a performance gain when using non-transactional queues, it should be carefully weighed against the possibility of message loss. 

snippet: use-nontransactional-queues

Note: This setting is not the same as using [TransactionMode.None](/transports/transactions.md#transactions-unreliable-transactions-disabled). This setting implies that the physical queue where the messages will be stored will not be a transactional queue. Endpoints that use non-transactional queues will not be able to send messages to endpoints that use transactional queues. Therefore it is important for all endpoints that intend to communicate with each other to use the same setting.
 
