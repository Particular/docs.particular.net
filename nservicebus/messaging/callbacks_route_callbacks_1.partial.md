
Callback responses are routed based on the `ReplyTo` header of the request. In order to use callbacks, instance Id needs to be explicitly specified in the requester endpoint configuration:

snippet: Callbacks-InstanceId

The endpoint queue name will include the specified suffix. When upgrading from lower versions the original input queue (without a suffix) will still exist, and the additional queue with the suffix will be created. 

Uniquely addressable endpoints can consume messages from both shared and unique queues. Replies will automatically be sent to the instance specific queue.

This approach makes it possible to deploy multiple callback-enabled instances of a given endpoint even to the same machine.

WARNING: This Id needs to be stable and it should never be hardcoded. An example approach might be reading it from the configuration file or from the environment (e.g. role Id in Azure).