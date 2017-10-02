---
title: Message Correlation
summary: Correlation is needed in order to find existing saga instances based on data on the incoming message
component: Core
reviewed: 2016-08-30
tags:
 - Saga
related:
 - nservicebus/sagas/concurrency
---


Correlation is needed in order to find existing saga instances based on data on the incoming message. In the example the `OrderId ` property of the `CompleteOrder` message is used to find the existing saga instance for that order.

To declare this use the `ConfigureHowToFindSaga` method and use the `Mapper` to specify to which saga property each message maps to.

NOTE: NServiceBus  only allows message mappings where the message properties correlate to a single saga property and have the same type. 

partial: note

snippet: saga-find-by-property

When `MyMessage` arrives, NServiceBus asks the saga persistence infrastructure to find an object of the type `MySagaData` that has a property `SomeId` whose value is the same as the `SomeId` property of the message. If found the saga instance will be loaded and the `Handle` method for the `MyMessage` message will be invoked. Should the saga instance not be found and the message not be allowed to start a saga, the [saga not found](saga-not-found.md) handlers will be invoked.


## Message Property Expression

If correlating on more than one saga property is necessary, or matched properties are of different types, use a [custom saga finder](saga-finding.md).


partial: expression


## Auto correlation

A common usage of sagas is to have them send out a request message to get some work done and receive a response message back when the work is complete. To make this easier NServiceBus will automatically correlate those response messages back to the correct saga instance.

NOTE: If it's not clear if the message can be auto correlated, it's better to provide the mappings. In case the message will be auto correlated, the mappings will be ignored.

NOTE: A caveat of this feature is that it currently doesn't support auto correlation between sagas. If the request is handled by a another saga relevant message properties must be added and mapped to the requesting saga using the syntax described above.


## Custom saga finder

Full control over how a message is correlated can be achieved by create a custom [saga finder](/nservicebus/sagas/saga-finding.md).


## Uniqueness

NServiceBus will make sure that all properties used for correlation are unique across all instances of the given saga type. How this is enforced is up to each persister but will most likely translate to a unique key constraint in the database.

Mapping a single message to multiple saga instances is not supported. This can be simulated by using a message handler that looks up all saga instance affected and send a separate message targeting each of those instances using the regular correlation described above.

partial: unique
