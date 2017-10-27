
Callbacks are tied to the endpoint instance making the request so all responses needs to be routed back to the specific instance making the request. This means that callbacks requires the endpoint to configure a unique instance Id:

snippet: Callbacks-InstanceId

This will make each instance of the endpoint uniquely addressable by creating an additional queue using the instance Id in the name of the queue. 

Selecting the Id depends on the transport used and how the endpoint is [scaled out](/transports/scale-out.md#when-to-scale-out).

For broker transports like Azure SeviceBus, RabbitMQ etc the instance Id needs to be selected unique across all instances since they all connect to the same broker.

For federated transports like MSMQ it might be enough to use a "hardcoded" Id like `replies`, `callbacks` etc if you are running each instance on a separate machine. 

Uniquely addressable endpoints will consume messages from both the shared and unique queue. Replies will automatically be routed to the instance-specific queue.

WARNING: To avoid creating excessive amounts of queues the Id needs to be keept stable. An example approach might be reading it from the configuration file or from the environment (e.g. role Id in Azure or machine name for on-premise deployments).
