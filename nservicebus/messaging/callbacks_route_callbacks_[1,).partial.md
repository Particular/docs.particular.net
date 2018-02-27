
Callbacks are tied to the endpoint instance making the request, so all responses need to be routed back to the specific instance making the request. This means that callbacks require the endpoint to configure a unique instance Id:

snippet: Callbacks-InstanceId

This will make each instance of the endpoint uniquely addressable by creating an additional queue that includes the instance Id in the name. 

Selecting an appropriate Id depends on the transport being used and whether or not the endpoint is [scaled out](/transports/scale-out.md#when-to-scale-out):
- For broker transports like Azure ServiceBus, RabbitMQ, etc., the instance Id needs to be unique for each instance, otherwise the instances will end up sharing a single callback queue and a reply could be received by the wrong instance.
- For federated transports like MSMQ, where every instance is running on a separate machine and can never share queues, then it is OK to use a single Id like `replies`, `callbacks`, etc.

Uniquely addressable endpoints will consume messages from their dedicated, instance-specific queues in addition to the main queue that all instances share. Replies will automatically be routed to the correct instance-specific queue.

WARNING: To avoid creating an excessive number of queues, the Id needs to be kept stable. For example, it may be retrieved from a configuration file or from the environment (e.g. role Id in Azure or machine name for on-premises deployments).
