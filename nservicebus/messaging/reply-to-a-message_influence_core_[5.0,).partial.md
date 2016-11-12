## Influencing the reply behavior

A sender of a reply can influence how the requester will behave when continuing the conversation (replying to a reply). It can request a reply to go to itself (not any other instance of the same endpoint)

snippet:BasicReplyReplyToThisInstance

or explicitly to any instance of the endpoint (which overrides the *public reply address* setting)

snippet:BasicReplyReplyToAnyInstance

It can also request the reply to be routed to a specified transport address instead

snippet:BasicReplyReplyToDestination
