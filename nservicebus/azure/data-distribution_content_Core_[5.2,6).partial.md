snippet: UniqueQueuePerEndpointInstance

The code above configures NServiceBus to use a unique input queue for each physical endpoint instance. Each queue name is the endpoint name suffixed by the Azure role instance ID. Since each subscriber has its own unique input queue, the publisher treats them as if they are separate logical subscribers and delivers events to all of them.

Alternatively, a custom queue name suffix can be specified (instead of using the Azure role instance ID):

snippet: UniqueQueuePerEndpointInstanceWithSuffix
