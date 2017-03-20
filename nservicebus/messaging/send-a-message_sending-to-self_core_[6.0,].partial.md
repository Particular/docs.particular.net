## Sending to *self*

Sending a message to the same endpoint, i.e. Sending to *self*, can be done in two ways.

An endpoint can send a message to any of its own instances:

snippet: BasicSendToAnyInstance

Or, it can request a message to be routed to itself, i.e. the same instance.

NOTE: This option is only possible when endpoint instance ID has been specified.

snippet: BasicSendToThisInstance