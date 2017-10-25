
### UseNonTransactionalQueues

This setting should be used with caution. As the queues are not transactional, any message that has an exception during processing will not be rolled back to the queue. Therefore this setting must only be used where loss of messages is an acceptable scenario. 

While there may be a performance gain when using non-transactional queues, it should be carefully weighed against message loss. 

snippet: use-nontransactional-queues

WARN: Endpoints that use non-transactional queues will not be able to send messages to endpoints that use transactional queues. 
 


