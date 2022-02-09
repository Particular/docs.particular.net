---
title: Evolving Message Contracts
summary: Guidelines for choosing a strategy for evolving message contracts
reviewed: 2022-02-10
component: Core
isLearningPath: true
---

In message-based systems, messages are part of a contract, which defines how services communicate with each other.

Evolving contracts over time is challenging and an appropriate strategy should be reviewed and customized for each system. When evolving message contracts, consider the following:

* At the time endpoints are upgraded to use new versions of a message contract, there may still be messages in-flight (that is, waiting in input queues to be processed) that use the old version of the contract. As a consequence:
  * Endpoints that receive or subscribe to messages and that are updated to a later version of the message contract might still receive messages that use the old version of the message contract
  * Endpoints that send or publish messages and that are updated to a later version of the message contract may send messages to endpoints that are expecting the old version of the message contract

Generally, the problem can't be resolved at the infrastructure level; therefore, NServiceBus users must analyze their systems, consider how they are expected to evolve, and define a strategy which will make the most sense in their particular circumstances.

This article presents some guidelines for choosing a contract evolution strategy, avoiding common mistakes, and ensuring that contracts will be easy to evolve over time.


TODO: link to new document
Note: Ensure that message contracts follow the general [messages design guidelines](/nservicebus/messaging/messages-events-commands.md#designing-messages).

## Message evolution scenarios

Message contracts evolve in several ways:

* A new message contract is added. This is essentially a brand new message type
* A property is added to an existing message contract
* A property is removed from an existing message contract

Note: Message properties can be renamed. From the perspective of the message contract, this is equivalent to removing a property and adding a new one.

In addition, there are different strategies that can be used to update a message contract when changes are required to it:

* Create a new version of the contract class in the same assembly and namespace with a different name (e.g. `OrderCreatedV2`)
* Create a new version of the contract class in a new namespace (with the same name or a different one)
* Use inheritance (e.g. `OrderCreatedV2` inherits from `OrderCreated`)
* Update the original contract class directly

Each option has its pros and cons which are discussed further in this document.

## Adding new message contracts

Adding a brand new message contract is a simple change. This is what happens when the first version of the system is deployed; all message contracts are new.

In this scenario, it's most effective to deploy endpoints that receive or subscribe to the new message contracts first. This ensures that all messages will be handled. If the sender/publisher is deployed first, messages could be sent without having an endpoint to receive them.

## Adding properties to existing contracts

Adding additional properties to existing contracts is a way to enhance a contract. Typically, new properties represent information that is required to implement new functionality in a system. As discussed earlier, there are different options for implementing a new property on an existing contract

### Create a new contract type

In this scenario, a completely new class is created starting from a copy of the original version of the contract. Typically a postfix is added to the class name (e.g. `CreateOrderV2`) to connect the contract with the original, at least logically. The new properties are then added to the new message contract and endpoints are configured to send and receive the new message type.

In a typical case, senders and publishers are updated to send or publish the new version of the contract. On the receiving/subscribing end, typically a separate handler is created to handle the new version of the contract and the existing handler remains as well so that it can continue handling old versions of the contract that may still be in-flight. Eventually, the old receivers/publishers can be decommissioned once it's verified that there are no versions of the old message contract in place.

Similar to adding a new message contract, it's recommended to update the receivers first so that no messages are missed if, for example, a sender sends a new version of a message type when there is no receiver set up to handle it yet.

TODO: what about version namespaces?

### Add new properties to existing contract types

In this scenario, new properties are added to the existing contract type; no new class is created. The recommended flow here is:

1. Add the new properties to the existing type
1. Update senders to the new contract version, setting the additional data on the message
1. Update receivers to handle the new contract version.

This approach requires less modifications to the contracts and even allows to update senders and receivers in arbitrary order as long as the receivers can handle the absence of a value for the new property, and as long is it's acceptable for senders to send new versions of the contract to receivers that are still handling the old contract versions.

TODO: can serialzers handle the absence?

It is recommended to use nullable types for the new properties to allow receivers to identify whether it is dealing with an old version of the contract.

### Use inheritance to create a new sub-type

Inheritance involves creating a new class that inherits from a previous version of the contract. For example, `OrderCreatedV3` may inherit from `OrderCreatedV2` and have a new property, say, `TaxCode` included on it. The workflow is:

1. Create a new contract type, inheriting from the contract type that should be extended
1. Add the new properties to the new sub-type
1. Update the senders to publish/send the new sub-type
1. Update the receivers to the new sub-type

This approach requires that all senders/publishers be updated and deployed first. Due to message inheritance, receivers can continue to process the original message version until they are updated.

## Removing properties and contract types

Removing a property may involve more planning than adding a property or contract. In some cases, it may involve keeping older versions of handlers around until there are no in-flight messages that use the old version of the contract.

One possible workflow is:

1. Update and deploy all receivers/publishers so that they no longer make use of the property to be removed
1. Remove the property from the contracts
1. Update senders/publishers to the new contracts assembly and deploy them

At this point, the sender is sending the new version of the message contract with the property removed. The receiver is also aware of the new contract type and functions normally. If there are any in-flight messages that still refer to the old message type, most serializers will still reconstitute the message correctly but without the removed property.

The Obsolete attribute is a useful tool for planning to remove a message property. It can be used to mark properties to be removed in advance of updating senders in order to give consumers of the contract time to update their code.

Removing a message type is similar to removing a property and the same workflow applies:

1. Update and deploy all receivers/publishers so that they no longer handle the contract to be removed
1. Remove the contract
1. Update senders/publishers to the new contracts assembly (without the contract that was removed) and deploy them

This scenario is more straightforward in that it doesn't matter the order in which the receivers and senders are deployed. In both cases, the message contract that is being removed is no longer processed, either because it's not being sent by a sender, or because it's not being handled by a receiver. Again, be aware of in-flight messages. If the handlers for them are removed, they may remain in the system indefinitely since there are no longer any handlers for them.

## Versioning

* Keep the assembly version on 1.0.0 to avoid assembly loading conflicts when endpoints with different contract versions send messages to each other
* Use file version and/or NuGet package versions to indicate the sematic version of the contracts assembly

## Modifying serialization formats

Another approach for handling breaking changes is to modify serialization formats. Step-by-step guidance is provided in the [transition serialization formats](/samples/serializers/transitioning-formats/) sample.
