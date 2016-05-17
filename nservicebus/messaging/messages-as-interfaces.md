---
title: Messages as Interfaces
summary: ' NServiceBus allows supports the use interfaces as well as standard XSD and class serialization.'
tags: []
redirects:
- nservicebus/messages-as-interfaces
---

Message schema flexibility is at the core of NServiceBus. Beyond just standard XSD and class serialization, NServiceBus support using interfaces as well.

One advantage of using interfaces to define messages instead of classes is that "multiple inheritance" is supported; i.e., one message can extend multiple other messages. This is useful for solving a specific class of versioning problems.

Say that the business logic represents a state machine with states X and Y. When the system gets into state X, it publishes the message `EnteredStateX`. When the system gets into state Y, it publishes the message `EnteredStateY`. (For more information on how to publish a message, see below.)

In the next version of the system, add a new state Z, which represents the co-existence of both X and Y. So, define the message `EnteredStateZ`, which inherits both `EnteredStateX` and `EnteredStateY`.

When the system publishes `EnteredStateZ`, clients subscribed to `EnteredStateX` and/or `EnteredStateY` are notified.

Without the ability to extend a message to multiple others, composition would be required, thereby preventing the infrastructure from automatically routing messages to pre-existing subscribers of the composed messages.

The versioning advantages of NServiceBus message types allow for easily extending endpoints without breaking clients or subscribers.


## Use classes for commands

When writing commands, it is recommended to use classes rather than interfaces for those messages. The advantage of using a class is that it is possible to include validation logic in the class constructor so that clients sending these messages are not able to send invalid commands. Of course, re-validating these commands can be done on the server side anyway.


## Use interfaces for events

Since events represent something that occurred in the past, the importance of validation is much decreased. Also, only the publisher of an event is the one that creates and sends the event. Subscribers cannot invalidate an event that was published. As such, the ability to put logic in the class representing a message is less relevant for events.

The other reason to use interfaces is that they enable multiple inheritance, which is very useful for extending a system over time from one version to the next, without breaking existing subscribers.

Assume that in version 1 of a publisher, it exposed an event of type X. In Version 2, a new event of type Y is added. Then in Version 3, a new requirement comes along for a type of event, Z, which means that both X and Y occurred. If X, Y, and Z were implemented as interfaces, Z could inherit from both X and Y. The main advantage of this is that when Z is published, subscribers of X and Y receive the event with no changes required of them, as well as subscribers of Z, of course.

This would not be possible using classes.