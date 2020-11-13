## Using registered endpoints

In order to route message to endpoints without having to specify the destination at all times, it is also possible to register the endpoint for a given command type, assembly or namespace

snippet: storage_account_routing_registered_endpoint

Once the endpoint is registered no send options need to be specified.

snippet: storage_account_routing_send_registered_endpoint

### Publishers

Similar to sending to an endpoint, the transport can also be configured to subscribe to events published by endpoints in another storage account, using:

snippet: storage_account_routing_registered_publisher
