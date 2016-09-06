---
title: Saga changes in Version 6
tags:
 - upgrade
 - migration
---


## Remove NServiceBus.Saga namespace

The `NServiceBus.Saga` namespace has been removed to stop it clashing with the `NServiceBus.Saga.Saga` class. For all commonly used APIs (eg the `Saga` class and `IContainSagaData ` interface) they have been moved into the `NServiceBus` namespace. Other more advanced APIs (eg the `IFinder` and `IHandleSagaNotFound` interfaces) have been moved into the `NServiceBus.Sagas` namespace.

In most cases `using NServiceBus.Saga` can be replaced with `using NServiceBus`.


## Unique attribute no longer needed

NServiceBus will automatically make the correlated saga property unique without the need for an explicit `[Unique]` attribute to be used. This attribute can be safely removed from saga data types.


## ConfigureHowToFindSaga

All messages that start the saga (`IAmStartedByMessages<T>`) need to be mapped to the saga data using either a mapping in `ConfigureHowToFindSaga` method, or a custom [saga finder](/nservicebus/sagas/saga-finding.md), otherwise an exception will be thrown on endpoint startup. Other messages that are handled by the saga (`IHandleMessages<T>`) also require mappings, unless they are reply messages resulting from a message sent out of the saga, in which case they will contain the SagaId in a message header. Messages that cannot be mapped by a SagaId message header, by a property mapping in `ConfigureHowToFindSaga`, or via a custom saga finder will throw a runtime exception.

In the below example, the `OrderSaga` is started by the `StartOrder` message. The `OrderSaga` also handles the `CompleteOrder` message.

snippet:5to6-SagaDefinition

In Version 6, the `StartOrder` message will also need to be specified in the `ConfigureHowToFindSaga` method.

snippet:5to6-ConfigureHowToFindSaga


## Correlating properties

Version 6 automatically correlates incoming message properties to its saga data counterparts. Any saga data correlation in the message handler code can be safely removed. Correlated properties (for existing saga instances) will not be changed once set.

snippet:5to6-NoSagaDataCorrelationNeeded

Version 6 will only support correlating messages to a single saga property. Correlating on more than one property is still supported by creating a custom [saga finder](/nservicebus/sagas/saga-finding.md).


## Saga persisters & finders

Saga persisters (`ISagaPersister`) and finders (`IFindSagas`) introduce a new parameter `SagaPersistenceOptions`. This parameter gives access to the saga metadata and pipeline context. This enables  persisters and finders to manipulate everything that exists in the context during message pipeline execution. For more information see [Sagas](/nservicebus/sagas/) and [Complex saga finding logic](/nservicebus/sagas/saga-finding.md).


## MarkAsComplete no longer virtual

The `Saga` base class method `MarkAsComplete` is no longer virtual.


## RequestTimeout requires IMessageHandlerContext

`RequestTimeout` requires a `IMessageHandlerContext` as additional parameter. Pass the context argument received in the handle method to `RequestTimeout`.


## ReplyToOriginator requires IMessageHandlerContext

`ReplyToOriginator` requires a `IMessageHandlerContext` as additional parameter. Pass the context argument received in the handle method to `RequestTimeout`.