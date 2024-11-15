---
title: Sharing message contracts
summary: How to define and share message contracts between endpoints.
component: Core
reviewed: 2024-10-31
related:
 - nservicebus/messaging/messages-events-commands
 - nservicebus/messaging/unobtrusive-mode
---

To minimize the amount of information and dependencies shared between endpoints, it's recommended to use separate assemblies for message contracts. It's also recommended to use a separate message contract assembly for each service. This allows a service to [evolve its contracts](/nservicebus/messaging/evolving-contracts.md) without impacting other services in the system. A message contract should be declared in the message contracts assembly of the service which owns the message contract.

> [!NOTE]
> Message contracts should also follow the [general message design guidelines](/nservicebus/messaging/messages-events-commands.md#designing-messages).

## Sharing contracts

A sender and receiver of a given message must use the same message contract. Message contracts may be shared in various ways:

* When all endpoints are located in a single solution, message contract assemblies may be directly referenced as project dependencies.
* When endpoints are located in multiple solutions, message contracts may be shared as [NuGet packages](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package-msbuild). NuGet packages may be published on the file system or a NuGet server. A [custom NuGet config file](https://docs.microsoft.com/en-us/nuget/reference/nuget-config-file) may be used to configure additional NuGet package sources.
* Messages contracts may be shared as C# source files.

### NServiceBus dependency

When [marker interfaces](/nservicebus/messaging/messages-events-commands.md#identifying-messages-marker-interfaces) are used to define messages, message contract projects have a dependency on the NServiceBus package. This may cause version conflicts when message contracts are updated in endpoints targeting older major versions of NServiceBus. This may be avoided by either:

* Use the [`NServiceBus.MessageInterfaces` package](/samples/message-assembly-sharing/) available from Version 8.2 onwards.
* Referencing the oldest used NServiceBus major version from message contracts projects. [NuGet dependency resolution](https://docs.microsoft.com/en-us/nuget/concepts/dependency-resolution) allows endpoints on newer major versions of NServiceBus to reference assemblies that target an older version of NServiceBus, but not vice versa.
* Switching to [unobtrusive mode](/nservicebus/messaging/unobtrusive-mode.md). Unobtrusive mode allows an assembly to define message contracts without a dependency on the NServiceBus package, making it easy to share message contracts with endpoints targeting multiple versions of NServiceBus and running on various frameworks and platforms.

> [!NOTE]
> Starting with version 8, NServiceBus no longer targets .NET Standard. [Multi-targeting](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/cross-platform-targeting#multi-targeting) must be used to support multiple target frameworks.

## Versioning

When endpoints are updated independently from each other, it is important to take into account the message contracts shared between them (or even between [endpoint instances](/nservicebus/endpoints/)).

After updating message contracts in an endpoint:

* Other endpoints must be able to process messages sent or published by the updated endpoint.
* The updated endpoint must be able to process messages sent or published by other endpoints.

NServiceBus includes a message header containing the message type's fully qualified name, which includes the assembly name and version number. Therefore, for best compatibility, it is recommended to maintain a stable [*assembly version*](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/versioning#assembly-version). The version of the contracts may be stated in the [*assembly file version*](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/versioning#assembly-file-version), [*assembly informational version*](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/versioning#assembly-informational-version), or *NuGet package version*.

The [evolving message contracts guidance](/nservicebus/messaging/evolving-contracts.md) has more details on safely updating message contracts.

## Breaking down large contract assemblies

In the early stages of developing a system, combining all events, commands, and messages in a single contract assembly is often a good way to start. As the system grows, breaking down the contracts into smaller parts makes it easier to evolve a system safely.

For example, an endpoint may have many subscribers to its events, but only one or two endpoints sending it commands. A good way to evolve the command senders without requiring the subscribers to update their contract assemblies would be to separate the command and events into two assemblies:

* EndpointName.Commands
* EndpointName.Events

Or when specific events are subscribed to by endpoints managed by other teams, it may make sense to extract those contracts into a separate assembly or NuGet package.
