---
title: Evolving messages contracts
reviewed: 
component: Core
---

In message-based systems the messages are part of the contract, which defines how services communicate with each other.

Evolving contracts over time is not an easy task and the appropriate strategy should be defined individually for each system. Generally that problem can't be resolved on the infrastructure level, therefore NServiceBus users need to analyze their systems, consider how they evolve and define the strategy which will make the most sense in their particular circumstances.

This article presents basic guidelines for choosing contracts evolution strategy, avoiding common mistakes and ensuring the contracts will be easy to evolve over time.


## Designing contracts


### Messages

Messages should be:

- **POCO classes**.
- **Small and focused**, carrying only the minimum amount of information, should be split if they get too big.
- **Server a single purpose**, they shouldn't be used for anything else than carrying data, in particular shouldn't be used as data access objects, etc.


### Assemblies

The assembly containing messages becomes part of the contract between two endpoints, as both need to refer it.

The messages can be defined in more than one assembly and it's recommended for systems with multiple message types to keep them separately. Especially commands and events tend to evolve at different pace and be independent from each other, so they can be kept in separate assemblies. 


## Common challenges


### Default values

When a message is extended with additional properties it's necessary to carefully examine what will be the default values for those properties, especially if endpoints running in other versions don't recognize them.

TODO: specific example?


## Handling breaking changes in contracts


### Handling both versions of messages

Serializers API => not sure what that was about? Using custom serialization rules e.g. to ignore extra properties or to have safe defaults for missing information?


### Upgrading endpoints

add-drain-delete flow for the endpoint.

create a new message type when significant changes in class occur (removing property, changing the type of property, ...) => is that a point where adding version number in a class name makes sense?

