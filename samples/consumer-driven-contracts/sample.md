---
title: Consumer Driven Contracts
reviewed: 2018-02-19
component: Core
versions: '[6,)'
---

WARNING: Support for consumer driven contracts relies on serializers being able to support multiple inheritance. At this time, only the [XML](/nservicebus/serialization/xml.md) or [Newtonsoft JSON](/nservicebus/serialization/newtonsoft.md) serializers meet this requirement.

## Introduction to consumer driven contracts

This sample shows a [consumer driven contract](https://martinfowler.com/articles/consumerDrivenContracts.html)(CDC) approach to messaging. The essence of consumer driven contracts is that the ownership of the contract is inverted. Instead of the producer providing the definition, consumers define the contract they expect, and it's up to the producer to fulfill it.

In NServiceBus terminology, "producers" are called "publishers" and "consumers" are called "subscribers". Contracts translate to message contracts and are defined using plain C# types. To honor a consumer contract, the producer would make the relevant message contract inherit from the consumer contract type.


## Contracts as interfaces

CDC is the main reason that `interface` messages are supported in addition to classes. More than one consumer can provide a contract that would be satisfied by the same publisher message. This would not be possible if only classes were used because multiple inheritance isn't supported by C#. The solution is to use interfaces instead, which lets the publisher implement all relevant contract types on the same message type.

Assuming that the producers `MyEvent` would satisfy both `Consumer1Contract` and `Consumer2Contract`, it would look like this:

snippet: publisher-contracts

NOTE: One limitation of this approach is that two or more consumers can't require a property with the same name but different types because that wouldn't compile using C#.

## Full names instead of fully qualified assembly names

NOTE: This step is only required for the Newtonsoft JSON serializer.

By default, NServiceBus publishes messages with the fully qualified assembly names in the [enclosed message type header](/nservicebus/messaging/headers.md#publish-headers). CDC for the Newtonsoft JSON serializer requires duck typing to be able to load the locally defined contracts. This can be achieved by replacing the fully qualified assembly name with the full name only. The following behavior demonstrates this:

snippet: replace-fullnames

## Running the sample

Run the sample and notice how each consumer receives its contract when the producer publishes `MyEvent`.

NOTE: Sharing contract types between endpoints is a larger topic, and this sample is using linked files for simplicity. See the [message contracts documentation](/nservicebus/messaging/evolving-contracts.md) for more details.
