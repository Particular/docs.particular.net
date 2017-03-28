## Influencing the reply behavior

When a receiving endpoint replies to a message, the reply message will be routed to any instance of the sending endpoint by default. The sender of the message can also control how reply messages are received. 

To send the reply message to the specific instance that sent the initial message:

snippet: BasicSendReplyToThisInstance

To send the reply message to any instance of the endpoint:

snippet: BasicSendReplyToAnyInstance

The sender can also request the reply to be routed to a specific transport address

snippet: BasicSendReplyToDestination