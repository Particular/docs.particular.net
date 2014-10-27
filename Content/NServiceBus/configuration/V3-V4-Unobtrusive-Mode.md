---
title: Configuration API Unobtrusive Mode in V3 and V4
summary: Configuration API Unobtrusive Mode in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
- V3
- V4
---

The requirement to define messages, commands and events, inheriting from NServiceBus interfaces, `IMessage`, `ICommand` and `IEvent`, creates a strong dependency on the NServiceBus assemblies and can cause versioning issues. To completely overcome the problem, NServiceBus can run in unobtrusive mode, meaning that you do not need to mark your messages with any interface and at configuration time you can define messages, commands, and events for NServiceBus: 

* `DefiningMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered a message or not. 
* `DefiningCommandsAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered a command or not.
* `DefiningEventsAs(Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered an event or not.
* `DefiningExpressMessagesAs( Func<Type, Boolean> predicate )`: for each type found in the scanned assemblies, the given predicate will be invoked to evaluate if the type should be considered an [express message](/nservicebus/how-do-i-specify-store-forward-for-a-message).
* `DefiningTimeToBeReceivedAs( Func<Type, TimeSpan> timeToBeReceivedHandler )`: for each type found in the scanned assemblies, the given predicate will be invoked to determine the [time to be received](/nservicebus/how-do-i-discard-old-messages) of each message, if any. 

NServiceBus can also define special behaviors for some message properties:

* `DefiningEncryptedPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type, invoke the given predicate to determine if the property value should be encrypted before the message is delivered.
* `DefiningDataBusPropertiesAs( Func<PropertyInfo, Boolean> predicate )`: for each property of each type, invoke the given predicate to determine if the property value should be transported using the data bus instead of the defined transport.
                
To dive into the unobtrusive mode:

* [Unobtrusive Mode Messages](/nservicebus/unobtrusive-mode-messages).