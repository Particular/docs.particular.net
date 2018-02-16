---
title: Consumer Driven Contracts
reviewed: 2017-03-21
component: Core
versions: '[6,)'
---

WARNING: Support for Consumer Driven Contracts relies on serializers being able to support multiple inheritance. At this time this means either the [XML]((/nservicebus/serialization/xml.md) or the [Newtonsoft JSON](/nservicebus/serialization/newtonsoft.md) serializer

## Introduction to Consumer Driven Contracts

This sample shows a [consumer driven contracts](https://martinfowler.com/articles/consumerDrivenContracts.html)(CDD) approach to messaging. The essence of consumer driven contracts is that the ownership of the contract is inverted. Instead of the producer defining the contract consumers are now the ones defining the contract they expect and it's up to the producer to fulfill it.

In NServiceBus terminology "Producers" are called "Publishers" and "Consumers" are called "Subscribers". Contracts translates to message contracts and is defined using plain C# types. So to honor a consumer contract the producer would just make the relevant message contract inherit from the consumer contract type.


## Contracts as interfaces

CDD is the main reason that `interface` messages are supported in addition to classes and the reason is that more than one consumer can provide a contract that would be satisfied by the same publisher message. If only classes where used this would not be possible since multiple inheritance isn't supported by C#. The solution is to use interfaces instead since with interfaces the publisher could implement all relevant contract types on the same message type.

Assuming that the producers `MyEvent` would satisfy both `Consumer1Contract` and `Consumer2Contract` this would look like this:

snippet: publisher-contracts

NOTE: The limitation of this approach is two or more consumers requiring a property with the same name but different types since that wouldn't compile using C#.

## Fullnames instead of Fully Qualified assembly names

NOTE: This step is only required for Newtonsoft JSON serializer.

By default NServiceBus publishes messages with the fully qualified assembly names in the [enclosed message type header](/nservicebus/messaging/headers.md#publish-headers). Consumer Driven Contract for Newtonsoft Json requires ducktyping to be able to load the locally defined contracts. This can be achieved by replacing the fully qualified assembly name by the full name only. The following behavior demonstrates this:

snippet: replace-fullnames

## Running the sample

Run the sample and notice how both consumers receives its contract when the producer publishes the `MyEvent`.

NOTE: Sharing contract types between endpoints is a larger topic and this sample is using linked files for simplicity. See the [message contacts documentation](/nservicebus/messaging/evolving-contracts.md) for more details.