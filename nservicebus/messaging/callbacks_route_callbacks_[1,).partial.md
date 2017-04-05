
Callback responses are routed based on the `ReplyTo` header of the request. To use callbacks, a unique instance Id needs to be specified in the requester endpoint configuration:

snippet: Callbacks-InstanceId

This will make each instance of the endpoint uniquely addressable by creating an additional queue using the instance Id in the name of the queue. Each instance needs to use a different instance Id to guarantee they each get their own queue.

Uniquely addressable endpoints will consume messages from both the shared and unique queues. Replies will automatically be sent to the instance-specific queue.

This approach makes it possible to deploy multiple callback-enabled instances of a given endpoint even to the same machine.

WARNING: This Id needs to be stable, and it should never be hardcoded. An example approach might be reading it from the configuration file or from the environment (e.g. role Id in Azure).
