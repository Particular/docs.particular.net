---
title: Evolving messages contracts
reviewed: 
component: Core
---

In message-based systems the messages are part of the contract, which defines how services communicate with each other.

Evolving contracts over time is not an easy task and the appropriate strategy should be defined individually for each system. Generally that problem can't be resolved on the infrastructure level, therefore NServiceBus users need to analyze their systems, consider how they expect them to evolve and define the strategy which will make the most sense in their particular circumstances.

This article presents basic guidelines for choosing contracts evolution strategy, avoiding common mistakes and ensuring the contracts will be easy to evolve over time.


## Designing contracts


### Messages

Messages should ideally be:

- [**Plain Old CLR Objects** (POCO)](https://en.wikipedia.org/wiki/Plain_Old_CLR_Object).
- **Small and focused**. Messages should carry the minimum amount of information and should be split if they get too big.
- **Serve a single purpose**. Messages shouldn't be used for anything else than carrying data. For example, messages shouldn't be used as data access objects or as UI binding models.


### Assemblies

The assembly containing messages becomes part of the contract between two endpoints, as both need to refer it. It's recommended to use a dedicated assembly for messages definitions. By keeping messages in a separate assembly, the amount of information and dependencies shared between services is minimized. 

Messages definitions can be divided between multiple assemblies, which might be useful in more complex systems, e.g. to narrow down the number of contracts exposed to different services.


## Common challenges


### Default values

When a message is extended with additional properties it's necessary to carefully examine what will be the default values for those properties, especially if endpoints running in other versions don't recognize them. In particular, it's important to consider how clients might interpret the default value and provide appropriate guidelines for them.


### Handling breaking changes in contracts

There is a number of approaches when it comes to handling breaking changes in messages. One of the simplest is adding a version to the type name.

In some advanced scenarios it might be necessary to introduce breaking changes in serialization formats. The guidance is provided in the [Transition serialization formats](/samples/serializers/transitioning-formats/) formats.


#### Upgrading endpoints

Whenever there are significant changes in a message type, such as removing property, changing the property type, etc. the upgrade process should consist of the following steps:
- Add the upgraded endpoint that will handle new incoming messages.
- Drain the incoming queue, i.e. keep the old endpoint running until all messages in the old format are processed.
- Delete the old endpoint.
