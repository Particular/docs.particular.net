---
title: Sharing message contracts
summary: How to define and share message contracts between different endpoints.
component: Core
reviewed: 2022-02-02
related:
 - nservicebus/messaging/messages-events-commands
 - nservicebus/messaging/unobtrusive-mode
---

It's recommended to use dedicated assemblies for message contracts. By keeping message contracts in a separate assembly, the amount of information and dependencies shared between services is minimized. Furthermore, it's recommended to have a separate message assembly for every service. When doing so, a service can [evolve its contracts](/nservicebus/messaging/evolving-contracts.md) without impacting other services in the system. Every message contract should be declared in the contracts assembly of the service owning that message contract.

Note: Ensure that message contracts follow the general [messages design guidelines](/nservicebus/messaging/messages-events-commands.md#designing-messages).

## Sharing contracts

Sender and receiver of a message must use the same message contract. Message contracts can be shared in various ways:

* When all endpoints are located in a single solution, the message contract assemblies can be directly referenced as a project dependency.
* When endpoints are split into multiple solutions, message contracts can be shared as NuGet packages. [Create a NuGet package](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package-msbuild) for the contracts assembly and deploy it to a local folder or a NuGet server. Use a [custom NuGet config file](https://docs.microsoft.com/en-us/nuget/reference/nuget-config-file) to configure additional NuGet package sources.
* Messages can be shared as C# source files without packaging them into an assembly.

### NServiceBus.Core dependency

When using the [marker interfaces](/nservicebus/messaging/messages-events-commands#identifying-messages-marker-interfaces) to define messages, the message contract project will have a dependency on the NServiceBus package. This can limit the ability to share new versions of message contract assemblies with older endpoints, as this might produce version conflicts when trying to update the message contracts on endpoints targeting older major versions of NServiceBus. In order to share the latest contract assembly version with all endpoints, consider these options:

* Ensure that the message contract assembly always references the oldest used NServiceBus major version. Due to [NuGet dependency resolution](https://docs.microsoft.com/en-us/nuget/concepts/dependency-resolution), endpoints on newer major versions of NServiceBus can reference assemblies that target an older version of NServiceBus, but not vice versa.
* Switch to [unobtrusive mode](/nservicebus/messaging/unobtrusive-mode.md). Unobtrusive mode allows an assembly to define message contracts without a dependency on the NServiceBus package, making it easy to share message contracts across different versions and platforms.

Note: Starting with NServiceBus version 8, NServiceBus no longer targets .NET Standard. Switch to [multi-targeting](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/cross-platform-targeting#multi-targeting) to support multiple platforms.

## Versioning

When endpoints are updated independently from each other, it is important to take into account the message contracts shared between different endpoints (or even between different endpoint instances). When changing the message contracts, ensure that:

* Old endpoints are able to process messages sent/published by the updated endpoint.
* The updated endpoint is able to process messages sent/published by old endpoints.

NServiceBus includes the message's fully qualified assembly name (including the assembly version number) in the message headers. Therefore, it is recommended to maintain a stable *assembly version* for the best compatibility. The version of the contracts can be tracked via the assembly *file version* and/or the *NuGet package version*.

See the [evolving message contracts](/nservicebus/messaging/evolving-contracts.md) guidance for more details on safely updating message contracts.

## Breaking down large contract assemblies

In the early stages of a system, combining all events, commands and messages into a single contracts assembly is often a good place to start. As the system grows, breaking down the contracts into smaller parts makes it easier to evolve a system safely, for example, a rise in the number of subscribers for events published by a specific endpoint. It's not desirable to expose commands that are meant to be consumed by a single receiver to all the subscribers interested in that same endpoint's events.

Consideration should be given to how the contracts assembly will be used by services. When certain events are subscribed to by multiple endpoints managed by other teams, it might make sense to extract those contracts into a separate NuGet package.

Depending on the usage and the frequency of changes, separating contracts into multiple assemblies might decouple these contracts and minimize the impact of the changes. A possible structure is to have multiple assemblies per endpoint with a naming convention similar to:

* EndpointName.Commands
* EndpointName.Events
