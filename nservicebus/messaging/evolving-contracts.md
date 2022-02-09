---
title: Evolving Message Contracts
summary: Guidelines for choosing a strategy for evolving message contracts
reviewed: 2022-02-10
component: Core
isLearningPath: true
---

In message-based systems, the messages are part of a contract, which defines how services communicate with each other.


Evolving contracts over time is challenging and an appropriate strategy should be reviewed and customized for each system. When evolving message contracts, consider the following:


* Endpoints updated to the latest message contract might still receive messages using the old contract. Senders might still use the old contract, or not all in-flight messages (messages waiting to be consumed in input queues) have been processed before the upgrade.

* Endpoints updated to the latest message contract might send messages, using the new contract, to endpoints still based on the old contract version.


Generally, the problem can't be resolved at the infrastructure level; therefore, NServiceBus users must analyze their systems, consider how they are expected to evolve, and define the strategy which will make the most sense in their particular circumstances.

This article presents basic guidelines for choosing a contract evolution strategy, avoiding common mistakes, and ensuring that contracts will be easy to evolve over time.


TODO: link to new document
Note: Ensure that message contracts follow the general [messages design guidelines](/nservicebus/messaging/messages-events-commands.md#designing-messages).

## Adding new message contracts

When adding new message contracts to a contracts assembly, update the endpoints receiving or subscribing to the new contract type first. If the sender/publisher endpoints are updated first, receivers and subscribers will not process messages for the new contract type.

## Adding data to existing contracts

Adding additional data to existing contracts is the most common change of contracts. There are different approaches available.

### Create a new contract type

1. Similar to adding a completely new contract using the original contracts name + some version postfix (e.g. `CreateOrderV2`), create a new message contract by copying the existing contract and adding the additional data to the copy.
1. Add message handlers for the new contract type to the receiving endpoints. Keep the existing mesage handlers for the "old" contract.
1. Update the sender/publisher to send messages using the new contract type.
1. Once all endpoints have been updated, and no more messages using the old contract type is left in the queue, the old type and associated handlers can be safely removed.

This approaches requires all receivers to be updated first, as they will be aware of the new contract type before upgrading which results in message processing failures.

TODO: what about version namespaces?

### Add new properties to existing contract types

1. Add the new properties to the existing type.
1. Update senders to the new contract version, setting the additional data on the message
1. Update receivers to handle the new contract version.

This approach requires less modifications to the contracts and even allows to update senders and receivers in arbitrary order as long as the receivers can handle the absence of a value for the new property.
TODO: can serialzers handle the absence?  

It is recommended to use nullable types for the new properties to allow receivers to identify whether it is dealing with an old version of the contract.

### Use inheritance to create a new sub-type

1. Create a new contract type, inheriting from the contract type that should be extended
1. Add the new properties to the new sub-type
1. Update the senders to publish/send the new sub-type
1. Update the receivers to the new sub-type

This approach requires all senders/publishers to be updated first. Due to message inheritance, receivers continue to process the original message version.

## Removing contracts or properties

1. Update all receivers/publishers to no longer use the property or message type to be removed
1. Remove the contract or property from the contracts
1. Update senders/publishers to the new contracts assembly.

The Obsolete attribute can be used to mark properties/types to be removed beforehand to give consumers of the contract time to update their code.

## Versioning

* Keep the assembly version on 1.0.0 to avoid assembly loading conflicts when endpoints with different contract versions send messages to each other
* Use file version and/or NuGet package versions to indicate the sematic version of the contracts assembly


## Modifying serialization formats

Another approach for handling breaking changes is to modify serialization formats. Step-by-step guidance is provided in the [transition serialization formats](/samples/serializers/transitioning-formats/) sample.


--------------------------------------------------------------------
Different proposal:

## Techniques

There are different techniques to evolve message contracts, each bringing it's own advantages and disadvantages. When selecting a technique it's important to carefully plan the migration strategy, the needs of message receivers/subscribers and message senders/publishers.

### New message type

Ship multiple versions of a message contract in the same assembly by creating a new message type for the new contract version. The new type typically indicates its relationship via its name, e.g.

```
// first version of the message contract
public class CreateOrder : ICommand {
    ...
}

// new version of the message contract
public class CreateOrderV2 : Icommand{
    ...
}
```

* Consumers can be updated to the new contract version independent from updating the contract assembly, giving more flexibility in planning the upgrade process.
* Allows more flexibility about the contract changes as no type-level compatibility has to be ensured.

When updating commands/messages:
* Ensure the receiver of the message has a handler for both types to ensure it can process messages that are still in the queue or coming from endpoints still using the old contract.

When updating events:
* Update all subscribers to have handlers (and subscribe) to both events before changing the publisher.

### Use inheritance

Ship multiple versions of a message contract in the same assembly by using inheritance. The new contract version inherits from the previous version, gaining all its parent's properties.

```
// first version of the message contract
public class OrderCreatedEvent : IEvent {
    ...
}

public class OrderCreatedEventV2 : OrderCreatedEvent {
    ...
}
```

* Can only be used to make additive changes
* Flattening the inheritance chain / removing older versions is more difficult compared to the other options
* Something about multiple/all message handlers being invoked
* Something about routing complexity?

When updating commands/messages:
* Update all senders to the new version before adding a new message handler for the new version. This might delay the time till the new message contract can be implemented by the receiver significantly.

When updating events:
* Update the publisher first to publish only the latest version of the event. Subscribers using the old message contract will also receive the event but process it the same as the previous version of the contract.

### Modify contract (adding data)

When modifying an existing contract, contract consumers updating to the latest contract version will only know this version of the contract. When only making additive changes, the serialization behavior can be used to ensure backwards compatibility with older versions of the contract.

```
// first version of the message contract
public class CreateOrder : ICommand {
    ...
    // this property has been added in the latest version:
    public int? SomeProperty { get; set; }
}
```

* Endpoints using the old version of the contract can process newer versions since the additional data is just being ignored
* The new data should be using nullable types to ensure endpoints can distinguish between missing data and intentionally set default values (e.g., `0` when using `int`).
* Endpoints using the new version of the contract need to expect the new data to be missing if there are still messages in the queue when updating, or if endpoints can send/publish using the old contract.

When updating commands/messages:
* The receiver of the message should be able to handle messages that have the new property missing. If that is not possible, all senders must be updated first, potentially delaying the time till the new data can be fully used by the receiver.

When updating events:
* Update the publisher first. Subscribers using the old contract will simply ignore the additional data and can be updated one at a time.

### Modify contract (removing data)

When modifying an existing contract, contract consumers updating to the latest contract version will only know this version of the contract. When removing properties from a contract, extra care has to be taken since this can impact endpoints still referencing the old version of the contract that aren't able to handle the missing data.

When updating commands/messages:
* Update receivers first to ensure they are no longer depending on the property being removed. During this time, the senders might still send the additional data till they are updated.

When updating events:
* Update all subscribers first to ensure they are no longer depending on the property being removed. During this time, the publisher still need to provide the data being removed.

Note: Renaming properties on a contract is equivalent to removing one property and adding another property.