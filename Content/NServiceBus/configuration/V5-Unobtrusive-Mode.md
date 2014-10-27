---
title: Configuration API Unobtrusive Mode in V5
summary: Configuration API Unobtrusive Mode in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

The requirement to define messages, commands and events, inheriting from NServiceBus interfaces, `IMessage`, `ICommand` and `IEvent`, creates a strong dependency on the NServiceBus assemblies and can cause versioning issues. To completely overcome the problem, NServiceBus can run in unobtrusive mode, meaning that you do not need to mark your messages with any interface and at configuration time you can define messages, commands, and events for NServiceBus: 

Unobtrusive conventions configuration that can be accessed via the `Conventions()` method on the `BusConfiguration` instance. The `Conventions()` method will return an instance of the `ConventionsBuilder` class that exposes the following methods: 

* `DefiningMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered a message or not. 
* `DefiningCommandsAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered a command or not.
* `DefiningEventsAs(Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered an event or not.
* `DefiningExpressMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered an [express message](how-do-i-specify-store-forward-for-a-message).
* `DefiningTimeToBeReceivedAs( Func<Type, TimeSpan> timeToBeReceivedHandler )`: for each type found in the scanned assemblies, the given predicate will be invoked to determine the [time to be received](how-do-i-discard-old-messages) of each message, if any. 

NServiceBus can also define special behaviors for some message properties:

* `DefiningEncryptedPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type, invoke the given predicate to determine if the property value should be encrypted before the message is delivered.
* `DefiningDataBusPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type, invoke the given predicate to determine if the property value should be transported using the data bus instead of the defined transport.
                
To dive into the unobtrusive mode:

* [Unobtrusive Mode Messages](unobtrusive-mode-messages).