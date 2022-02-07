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
