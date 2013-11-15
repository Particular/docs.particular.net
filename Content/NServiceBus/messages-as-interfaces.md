<!--
title: "Messages as Interfaces"
tags: ""
summary: "Message schema flexibility is at the core of NServiceBus. Beyond just standard XSD and class serialization, NServiceBus allows you to use interfaces as well."
-->

Message schema flexibility is at the core of NServiceBus. Beyond just standard XSD and class serialization, NServiceBus allows you to use interfaces as well.

The versioning advantages of NServiceBus message types allow for easily extending endpoints without breaking clients or subscribers.

Use classes for commands
------------------------

When writing commands, it is recommended to use classes rather than interfaces for those messages. The advantage of using a class is that you can include validation logic in the class constructor so that clients sending these messages are not able to send invalid commands. Of course, you will re-validating these commands on the server side anyway.

Use interfaces for events
-------------------------

Since events represent something that occurred in the past, the importance of validation is much decreased. Also, only the publisher of an event is the one that creates and sends the event. Subscribers cannot invalidate an event that was published. As such, the ability to put logic in the class representing a message is less relevant for events.

The other reason to use interfaces is that they enable multiple inheritance, which is very useful for extending a system over time from one version to the next, without breaking existing subscribers.

Let's say that in version 1 of a publisher, it exposed an event of type X. In version 2, we added a new event of type Y. Then in version 3, a new requirement comes along for a type of event, Z, which means that both X and Y occurred. If X, Y, and Z were implemented as interfaces, we could have Z inherit both X and Y. The main advantage of this is that when Z is published, subscribers of X and Y receive the event with no changes required of them, as well as subscribers of Z, of course.

This would not be possible using classes.

