## Code walk-through

This sample contains three projects:

 * Shared - A class library containing common code including messages definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the `OrderSubmitted` message, sending `OrderAccepted` message and randomly generating exceptions.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end.


### Receiver project

The Receiver mimics a back-end system. It is configured to use SQL persistence in multi-tenant mode.
