---
title: Change Message Identity
reviewed: 2017-10-30
component: Core
related:
- nservicebus/serialization
- samples/serializers/transitioning-formats
---

This sample illustrates an approach for change a message identity. This includes any one, or multiple, of the following scenarios:

 * Moving a message type between assemblies.
 * Renaming a message type.
 * Renaming the assembly containing the message type.
 * Adding, removing or changing the [strong name](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/strong-named-assemblies) of the assembly containing the message type

NOTE: This sample uses 2 "Phase" Endpoint Projects to illustrate the iterations of a single endpoint in one solution.


## Scenario

This sample shows moving a message using multiple of the above mentioned scenarios.

**Phase 1**

In the Phase 1 endpoint the message is:

 * Named `CreateOrderPhase1`.
 * Exists in an assembly named `SamplePhase1`.
 * Exists in an assembly that is not strong named.


**Phase 2**

In the Phase 2 endpoint the message type is:

 * Named `CreateOrderPhase2`.
 * Exists in an assembly named `SamplePhase2`.
 * Exists in an assembly that is strong named.


## Mutation

This change is achieved via the use of a [IMutateIncomingTransportMessages](/nservicebus/pipeline/message-mutators.md?version=core_7#transport-messages-mutators-imutateincomingtransportmessages).

The mutator is registered at endpoint startup:

snippet: RegisterMessageMutator


The mutator then replaces the [NServiceBus.EnclosedMessageTypes header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) via the use of [Type.GetType(typeName, assemblyResolver, typeResolver)](https://msdn.microsoft.com/en-us/library/ee332932.aspx) API.

snippet: Mutator


## Running the sample

When the sample is run both endpoints will startup. The Phase1 endpoint will send a `CreateOrderPhase1` message to Phase2. Phase2 will then mutate the message into a `CreateOrderPhase2` and handle the message.
