
Callbacks are tied to the endpoint instance making the request, so all responses need to be routed back to the specific instance making the request. This means that callbacks requires the endpoint to configure a unique instance Id:

snippet: Callbacks-InstanceId

This will make each instance of the endpoint uniquely addressable, an additional queue will be created that has the instance Id in the name. 

Selecting the Id depends on the transport used and how the endpoint is [scaled out](/transports/scale-out.md#when-to-scale-out):
- For broker transports like Azure SeviceBus, RabbitMQ, etc. the instance Id needs to be unique across all instances since they all connect to the same broker.
- For federated transports like MSMQ, when every instance is running on a seprate machine, then it is enough to use a "hardcoded" Id like `replies`, `callbacks`, etc.. 

Uniquely addressable endpoints will consume messages from both the shared and their dedicated, instance-specific queues. Replies will automatically be routed to the correct instance-specific queue.

WARNING: To avoid creating excessive number of queues, the Id needs to be kept stable. For example, it may be retrieved from a configuration file or from the environment (e.g. role Id in Azure or machine name for on-premise deployments).
