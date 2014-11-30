---
title: Authoring a custom NServiceBus persistence
summary: How to author a custom NServiceBus persistence in NServiceBus v5
tags: []
---

NServiceBus requires a persistence mechanism to store data for some of it's features, as discussed in  [Persistence in NServiceBus](persistence-in-nservicebus.md).

While a variety of persistence technologies are supported out of the box by NServiceBus (for example SQL databases via NHibernate, or RavenDB) you sometimes may want to write your own persistence, for example in order to reuse a database / persistence technology already in your stack and used by other parts of your system, but is not supported by NServiceBus yet. As you will see, writing an NServiceBus persistence is quite a straight forward task.

This guide will explain the various tasks involved in writing a custom persistence. We will use the in-memory persistence which comes out of the box with NServiceBus to showcase a simple real-world implementation as we explain the various concepts and discuss potential pitfalls. The source code for the in-memory persistence implementation can be found [here](https://github.com/Particular/NServiceBus/tree/4.6.5/src/NServiceBus.Core/Persistence/InMemory).

The data persisted by NServiceBus needs to survive endpoint restarts, so it doesn't lose timeouts or important Saga data for example. All persister implementations provided by NServiceBus are durable, with the exception of the in-memory one which is used purely for testing. And every persister you will be writing should be durable and properly tested as well.

It is important to note writing a new persistence for NServiceBus does require a good knowledge of the underlying persistence technology used. Being familiar with its guarantees of consistency and durability, and its querying abilities, is very important. We will see why in just a minute.

## The Subscriptions Persister

The first and maybe the most obvious piece of data persisted by NServiceBus is subscriptions. A subscription defines which endpoint is subscribed to what message type, which is oftentimes a many-to-many relationship.

A subscription storage is defined by the following interface:

```csharp
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

The last 3 methods in this interface are quite self explanatory. Additionally, your implementation gets a chance to perform initialization steps. This can be used for example to set some schema if the underlying persistence technology expects one.

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

Persisting a Saga is really just a matter of serializing this class and storing it within the underlying persistent storage. However, note how Sagas are allowed to be pulled by various criteria (property name and value) and not only by their ID. This means you should pay attention to those methods and use indexes or whatever other method that makes sense with your persistent technology of choice to pull Sagas efficiently. Like before, favor read speed over write speed.

Another important aspect of Saga persistence is concurrency. By design, it is possible for Sagas to be accessed and ammended by more than one thread concurrently. This requires the Saga persister to allow for a strong consistency model, to ensure Sagas are written and updated in an atomic manner. Every persistence technology is going to have its own way of providing this ability; for example SQL databases provide ACID guarantees and allow for optimizations like the Upgrade Lock mode to allow for efficient and secure updates under lock. RavenDB however is an eventually-consistent storage, and as such it uses optimistic concurrency and some tricks to implement the unique constraint functionality. To learn more about this and what is required from the Saga persister, read the [NServiceBus Sagas And Concurrency article](nservicebus-sagas-and-concurrency.md).

The in-memory implementation of `ISagaPersister` can be found [here](https://github.com/Particular/NServiceBus/blob/4.6.5/src/NServiceBus.Core/Persistence/InMemory/SagaPersister/InMemorySagaPersister.cs). Note how the Get by property method is implemented inefficiently, iterating through all Sagas instead of using indexes. For production worthy persisters this should not be the case.

## Timeout persister

Another type of data being persisted by NServiceBus is timeouts. Because NServiceBus is not a scheduling framework there is no hard guarantee of timeouts firing at the exact moment they are scheduled for. However, timeouts should definitely not be missed or fired in a serious delay. This can get tricky with some persistence technologies, so this is definitely something you should consider and plan for.

Writing a timeout persister can be done by implementing the `IPersistTimeouts` interface shown below:

```csharp
/// <summary>
/// Timeout persister contract.
/// </summary>
public interface IPersistTimeouts
{
    /// <summary>
    /// Retrieves the next range of timeouts that are due.
    /// </summary>
    /// <param name="startSlice">The time where to start retrieving the next slice, the slice should exclude this date.</param>
    /// <param name="nextTimeToRunQuery">Returns the next time we should query again.</param>
    /// <returns>Returns the next range of timeouts that are due.</returns>
    IEnumerable<Tuple<string, DateTime>> GetNextChunk(DateTime startSlice, out DateTime nextTimeToRunQuery);

    /// <summary>
    /// Adds a new timeout.
    /// </summary>
    /// <param name="timeout">Timeout data.</param>
    void Add(TimeoutData timeout);

    /// <summary>
    /// Removes the timeout if it hasn't been previously removed.
    /// </summary>
    /// <param name="timeoutId">The timeout id to remove.</param>
    /// <param name="timeoutData">The timeout data of the removed timeout.</param>
    /// <returns><c>true</c> it the timeout was successfully removed.</returns>
    bool TryRemove(string timeoutId, out TimeoutData timeoutData);

    /// <summary>
    /// Removes the time by saga id.
    /// </summary>
    /// <param name="sagaId">The saga id of the timeouts to remove.</param>
    void RemoveTimeoutBy(Guid sagaId);
}
```

The `TimeoutData` class holds timeout related data, like the `Time` it needs to fire at and the `SagaId` it is associated with. As a general rule, you should not use this class directly for persistence, but use another persistence class when possible and use the unique ID generation offered by the persistence you use.

NServiceBus polls the persister for timeouts by calling `GetNextChunk`, and providing it with `DateTime startSlice` which specifies what is the last timeout it recieved in the previous call to this method, and then the persister should provide all timeouts that are due, meaning from that value to the current point in time. Some eventually consistent storages may require you to be innovative to make sure no timeouts are missed. Finally, the `nextTimeToRunQuery` needs to be set to tell NServiceBus when to next poll the persister for timeouts - usually this is set for the next known timeout after the current time. NServiceBus will automatically poll for timeouts again if it has reason to suspect new timeouts are available.

The in-memory implementation of `IPersistTimeouts` can be seen [here](https://github.com/Particular/NServiceBus/blob/4.6.5/src/NServiceBus.Core/Persistence/InMemory/TimeoutPersister/InMemoryTimeoutPersistence.cs).

## Outbox persister

The Outbox functionality, new in NServiceBus v5, is a feature providing reliable messaging on top of various transports without using MSDTC. You can read more about the Outbox feature in [Reliable messaging without MSDTC](no-dtc.md).

An Outbox persister is implementing the following interface:

```csharp
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

You can implement any of the persisters based on your requirements. None of them are mandatory, and you can even use different persistence technologies for different persistence concerns (like SQL Server for timeouts and RavenDB for Sagas). Once the persisters you need have been written and properly tested, you need to enable them using [Features](fluent-config-api-v3-v4-intro.md#features).

Once a persister has been written, tested and exposed via a Feature, all that is left to do is add a reference to the assembly containing it from your endpoints, and change the endpoint configuration accordingly to enable it. An example for such configuration would be:

```csharp
var configure = new BusConfiguration();
configure.UsePersistence<RavenDBPersistence>(); // Select which persistence to use
configure.EnableFeature<Sagas>(); // Enable a feature or several of them
configure.UseSerialization<JsonSerializer>(); // Some more global configurations
configure.EnableInstallers();
```

You could write extension methods to add more configuraitons specific to your custom persistence (for example, to allow fine tuning of various aspects of it from the calling endpoint).
