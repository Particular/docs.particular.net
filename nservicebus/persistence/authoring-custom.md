---
title: Authoring a custom persistence
summary: How to author a custom NServiceBus persistence in NServiceBus Version 5
redirects:
 - nservicebus/authoring-custom-nservicebus-persistence
---

NServiceBus requires a persistence mechanism to store data for some of it's features, as discussed in  [Persistence in NServiceBus](/nservicebus/persistence/).

A variety of persistence technologies are supported out of the box by NServiceBus (for example SQL databases via NHibernate, or RavenDB). However it is possible to write a custom persistence, for example in order to reuse a database / persistence technology already in the stack and used by other parts of the system, but is not supported by NServiceBus yet..

This guide explains the various tasks involved in writing a custom persistence. The in-memory persistence, which comes out of the box with NServiceBus, is used to showcase a simple real-world implementation and explain the various concepts and discuss potential pitfalls. The source code for the in-memory persistence implementation can be found [here](https://github.com/Particular/NServiceBus/tree/4.6.5/src/NServiceBus.Core/Persistence/InMemory).

The data persisted by NServiceBus needs to survive endpoint restarts, so it doesn't lose timeouts or important Saga data for example. All persister implementations provided by NServiceBus are durable, with the exception of the in-memory one which is used purely for testing.

It is important to note writing a new persistence for NServiceBus does require a good knowledge of the underlying persistence technology used. Being familiar with its guarantees of consistency and durability, and its querying abilities, is very important.


## The Subscriptions Persister

The first and maybe the most obvious piece of data persisted by NServiceBus is subscriptions. A subscription defines which endpoint is subscribed to what message type, which is oftentimes a many-to-many relationship.

A subscription storage is defined by the following interface:

```cs
/// <summary>
/// Defines storage for subscriptions
/// </summary>
public interface ISubscriptionStorage
{
    /// <summary>
    /// Notifies the subscription storage that now is the time to perform
    /// any initialization work
    /// </summary>
    void Init();

    /// <summary>
    /// Subscribes the given client address to messages of the given types.
    /// </summary>
    void Subscribe(Address client, IEnumerable<MessageType> messageTypes);

    /// <summary>
    /// Unsubscribes the given client address from messages of the given types.
    /// </summary>
    void Unsubscribe(Address client, IEnumerable<MessageType> messageTypes);

    /// <summary>
    /// Returns a list of addresses of subscribers that previously requested to be notified
    /// of messages of the given message types.
    /// </summary>
    IEnumerable<Address> GetSubscriberAddressesForMessage(IEnumerable<MessageType> messageTypes);
}
```

The last 3 methods in this interface are quite self explanatory. Additionally, the implementation gets a chance to perform initialization steps. This can be used for example to set some schema if the underlying persistence technology expects one.

As a general comment that is also valid to the other persisters in this guide, it is preferred to design the implementation in such a way that prefers reads over writes. That is, prefer doing more work in the `Subscribe` and `Unsubscribe` methods so `GetSubscriberAddressesForMessage` can execute faster, as it is the one that's going to get called the most.

An in-memory implementation of the `ISubscriptionStorage` interface can be seen [here](https://github.com/Particular/NServiceBus/blob/4.6.5/src/NServiceBus.Core/Persistence/InMemory/SubscriptionStorage/InMemorySubscriptionStorage.cs)


## Saga persister

Another obvious piece of data that needs to be persisted by NServiceBus is Saga data. Every class implementing `IContainSagaData` is going to be persisted by the Saga persister once the Saga it is associated with was initialized:

```csharp
/// <summary>
/// Defines the basic functionality of a persister for storing
/// and retrieving a saga.
/// </summary>
public interface ISagaPersister
{
	/// <summary>
	/// Saves the saga entity to the persistence store.
	/// </summary>
	/// <param name="saga">The saga entity to save.</param>
    void Save(IContainSagaData saga);

    /// <summary>
    /// Updates an existing saga entity in the persistence store.
    /// </summary>
    /// <param name="saga">The saga entity to updated.</param>
    void Update(IContainSagaData saga);

	/// <summary>
	/// Gets a saga entity from the persistence store by its Id.
	/// </summary>
	/// <param name="sagaId">The Id of the saga entity to get.</param>
    TSagaData Get<TSagaData>(Guid sagaId) where TSagaData : IContainSagaData;

    /// <summary>
    /// Looks up a saga entity by a given property.
    /// </summary>
    TSagaData Get<TSagaData>(string propertyName, object propertyValue) where TSagaData : IContainSagaData;

	/// <summary>
    /// Sets a saga as completed and removes it from the active saga list
	/// in the persistence store.
	/// </summary>
	/// <param name="saga">The saga to complete.</param>
    void Complete(IContainSagaData saga);
}
```

Persisting a Saga is really just a matter of serializing this class and storing it within the underlying persistent storage. However, note how Sagas are allowed to be pulled by various criteria (property name and value) and not only by their ID. Pay attention to those methods and use indexes or whatever other method that makes sense with the persistent technology of choice to pull Sagas efficiently. Like before, favor read speed over write speed.

Another important aspect of Saga persistence is concurrency. By design, it is possible for Sagas to be accessed and ammended by more than one thread concurrently. This requires the Saga persister to allow for a strong consistency model, to ensure Sagas are written and updated in an atomic manner. Every persistence technology is going to have its own way of providing this ability; for example SQL databases provide ACID guarantees and allow for optimizations like the Upgrade Lock mode to allow for efficient and secure updates under lock. RavenDB however is an eventually-consistent storage, and as such it uses optimistic concurrency and some tricks to implement the unique constraint functionality. To learn more about this and what is required from the Saga persister, read the [NServiceBus Sagas And Concurrency article](/nservicebus/sagas/concurrency.md).

The in-memory implementation of `ISagaPersister` can be found [here](https://github.com/Particular/NServiceBus/blob/4.6.5/src/NServiceBus.Core/Persistence/InMemory/SagaPersister/InMemorySagaPersister.cs). Note how the Get by property method is implemented inefficiently, iterating through all Sagas instead of using indexes. For production worthy persisters this should not be the case.


## Timeout persister

Another type of data being persisted by NServiceBus is timeouts. Because NServiceBus is not a scheduling framework there is no hard guarantee of timeouts firing at the exact moment they are scheduled for. However, timeouts should definitely not be missed or fired in a serious delay. This can get tricky with some persistence technologies.

Writing a timeout persister can be done by implementing the interfaces shown below:

snippet:PersistTimeoutsInterfaces

The `TimeoutData` class holds timeout related data, like the `Time` it needs to fire at and the `SagaId` it is associated with. As a general rule, do not use this class directly for persistence, but use another persistence class when possible and use the unique ID generation offered by the underlying persistence.

NServiceBus polls the persister for timeouts by calling `GetNextChunk`, and providing it with `DateTime startSlice` which specifies what is the last timeout it received in the previous call to this method, and then the persister should provide all timeouts that are due, meaning from that value to the current point in time. Some eventually consistent storages may require innovative to make sure no timeouts are missed. Finally, the `nextTimeToRunQuery` needs to be set to tell NServiceBus when to next poll the persister for timeouts - usually this is set for the next known timeout after the current time. NServiceBus will automatically poll for timeouts again if it has reason to suspect new timeouts are available.

In order to provide a custom timeout persister implementation:

 - In Versions 4.x (starting from 4.4) and Versions 5.x it is required to implement interfaces `IPersistTimeouts` and `IPersistTimeoutsV2`. The interface `IPersistTimeoutsV2` was introduced to prevent a potential message loss, while the `IPersistTimeouts` interface maintains backwards compatibility. More details can be found in the following [issue description](https://github.com/Particular/NServiceBus/issues/2885).
  - The reference in-memory implementation of timeouts persistence for NServiceBus Versions 4.x can be seen [here](https://github.com/Particular/NServiceBus/blob/support-4.4/src/NServiceBus.Core/Persistence/InMemory/TimeoutPersister/InMemoryTimeoutPersistence.cs).
  - The reference in-memory implementation of timeouts persistence for NServiceBus Versions 5.x can be seen [here](https://github.com/Particular/NServiceBus/blob/support-5.0/src/NServiceBus.Core/Persistence/InMemory/TimeoutPersister/InMemoryTimeoutPersister.cs).
 - Starting from Version 6.0 it is required to implement interfaces `IPersistTimeouts` and `IQueryTimeouts`. The interface `IQueryTimeouts` has been extracted from `IPersistTimeouts` in order to explicitly separate responsibilities.
  - The reference in-memory implementation of timeouts persistence for Version 6.0 can be seen [here](https://github.com/Particular/NServiceBus/blob/develop/src/NServiceBus.Core/Persistence/InMemory/TimeoutPersister/InMemoryTimeoutPersister.cs).


## Outbox persister

The Outbox functionality, new in NServiceBus Version 5, is a feature providing reliable messaging on top of various transports without using MSDTC. Read more about the Outbox feature in [Reliable messaging without MSDTC](/nservicebus/outbox/).

An Outbox persister is implementing the following interface:

```cs
/// <summary>
/// Implemented by the persisters to provide outbox storage capabilities
///
/// </summary>
public interface IOutboxStorage
{
    /// <summary>
    /// Tries to find the given message in the outbox
    /// </summary>
    bool TryGet(string messageId, out OutboxMessage message);

    /// <summary>
    /// Stores an array of operations under the provided messageId
    /// </summary>
    void Store(string messageId, IEnumerable<TransportOperation> transportOperations);

    /// <summary>
    /// Tells the storage that the message has been dispatched and its now safe to clean up the transport operations
    /// </summary>
    void SetAsDispatched(string messageId);
}
```

The Store method has to use the same persistence session as the user's code - the same one that is used for persisting his business data as well as any Sagas. Sharing the session is the only way NServiceBus can support the Outbox feature properly and with transactions.


## Enabling persisters via Features

Any of the persisters can be implemented based on the specific requirements. None of them are mandatory, and it is possible to use different persistence technologies for different persistence concerns (like SQL Server for timeouts and RavenDB for Sagas). Once the persisters is written it can be enableed via a Feature.

Once a persister has been written, tested and exposed via a Feature, all that is left to do is add a reference to the assembly containing it from the endpoints, and change the endpoint configuration accordingly to enable it. An example for such configuration would be:

```cs
var configure = new BusConfiguration();
configure.UsePersistence<RavenDBPersistence>(); // Select which persistence to use
configure.EnableFeature<Sagas>(); // Enable a feature or several of them
configure.UseSerialization<JsonSerializer>(); // Some more global configurations
configure.EnableInstallers();
```
Additional extension methods could be created to add more configurations specific to the custom persistence (for example, to allow fine tuning of various aspects of it from the calling endpoint).
