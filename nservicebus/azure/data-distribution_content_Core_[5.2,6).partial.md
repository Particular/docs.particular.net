snippet: UniqueQueuePerEndpointInstance

The code above instructs NServiceBus to use a unique input queue for each endpoint instance. The queue name is derived from the endpoint name and the Azure role instance ID. When instances of such endpoint subscribe for an event, each instance subscribes with its own unique input queue so the publisher treats each subscription individually and, as a result, broadcasts events to all subscribed instances.

In case there is a need to provide a custom queue names suffix (instead of using the role instance ID), following API can be used instead:

snippet: UniqueQueuePerEndpointInstanceWithSuffix