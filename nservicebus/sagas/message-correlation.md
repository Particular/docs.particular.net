---
title: Message Correlation
summary: Correlation is needed in order to find existing saga instances based on data in the incoming message
component: Core
reviewed: 2020-01-24
related:
 - nservicebus/sagas/concurrency
---


Correlation is needed in order to find existing saga instances based on data in the incoming message. For example, an `OrderId ` property of a `CompleteOrder` message can be used to find the existing saga instance for that order.

To declare this, use the `ConfigureHowToFindSaga` method and use the `Mapper` to specify which saga property each message maps to.

partial: note

snippet: saga-find-by-property

When an instance of `MyMessage` arrives, NServiceBus asks the saga persistence infrastructure to find an object of the type `MySagaData` that has a property `SomeId` whose value is the same as the `SomeId` property of the message. If found, the saga instance will be loaded and the `Handle` method for the `MyMessage` message will be invoked. Should the saga instance not be found, the saga is not started and the [saga not found](saga-not-found.md) handlers will be invoked.

partial: reverse-api

## Message property expression

If correlating on more than one saga property is necessary, or matched properties are of different types, use a [custom saga finder](saga-finding.md).

It is possible to specify the mapping to the message using expressions if the correlation information is split between multiple fields.

snippet: saga-find-by-expression

partial: find-by-header


## Auto-correlation

A common usage of sagas is to have them send out a request message to get some work done and receive a response message back when the work is complete. To make this easier NServiceBus will automatically correlate those response messages back to the correct saga instance.

NOTE: If it's not clear if the message can be auto-correlated, it's better to provide the mappings. In cases where the message will be auto-correlated, the mappings will be ignored.

NOTE: A caveat of this feature is that it currently doesn't support auto-correlation between sagas. If the request is handled by another saga, relevant message properties must be added and mapped to the requesting saga using the syntax described above.


## Custom saga finder

Full control over how a message is correlated can be achieved by creating a custom [saga finder](/nservicebus/sagas/saga-finding.md).


## Uniqueness

NServiceBus will make sure that all properties used for correlation are unique across all instances of the given saga type. How this is enforced is up to each persister but will most likely translate to a unique key constraint in the database.

Mapping a single message to multiple saga instances is not supported. This can be simulated by using a message handler that looks up all saga instance affected and send a separate message targeting each of those instances using the regular correlation described above.

partial: unique