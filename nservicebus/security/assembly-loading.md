---
title: Assembly loading
summary: Describes security-related aspects with assembly loading and how to control the endpoint's behavior further
component: Core
reviewed: 2024-10-31
related:
- nservicebus/security
- nservicebus/hosting/assembly-scanning
---

## Assembly scanning

NServiceBus relies on [assembly scanning](/nservicebus/hosting/assembly-scanning.md) to detect and load user-defined extension points into the endpoint. Depending on the configuration, endpoints will automatically load assemblies deployed to the configured probing paths. This behavior implies a high level of trust in assemblies being deployed along with the application and appropriate infrastructure configuration to prevent foreign access to the application's binary folders (e.g., using file system permissions and appropriate system accounts).

partial: disable-scanning

## Dynamic type loading

Due to the various ways of defining, sharing, and deploying message contracts, a specific message type might not be available or detected by assembly scanning at startup time. NServiceBus is capable of automatically loading message types, including their defining assembly, at runtime if it is present in the .NET runtime's assembly probing paths. This behavior can lead to additional assemblies being loaded during runtime, based on incoming messages `NServiceBus.EnclosedMessageTypes` header.

partial: disable-type-loading

> [!NOTE]
> Loading types do not automatically create instances of the loaded type. NServiceBus prevents instantiation of types that are not considered [valid message types](/nservicebus/messaging/conventions.md).
