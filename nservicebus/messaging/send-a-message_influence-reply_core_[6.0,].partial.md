

## Influencing the reply behavior

The sender of the message can also control how the reply messages are received. When a receiving endpoint replies to a message, by default the reply message will be routed to any instance of the sending endpoint.

To explicitly control the reply message to be dispatched to a particular instance:

snippet: BasicSendReplyToThisInstance

To send the reply message to any instance of the endpoint:

snippet: BasicSendReplyToAnyInstance

The sender can also request the reply to be routed to a specific transport address

snippet: BasicSendReplyToDestination

