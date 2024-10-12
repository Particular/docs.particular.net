---
title: Developing with the Platform
summary: Guidance on the different ways develop using the platform
reviewed: 2024-09-22
---

Developing with the Particular Service Platform can be done in a number of ways, depending on the local development environment.

## NServiceBus

NServiceBus is a NuGet package that is used by the development team to create endpoints that are [hosted](/nservicebus/hosting/) in nearly any [.NET compatible process](installation.md#installation-nservicebus) using [the C#/.NET SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks).

The NServiceBus NuGet packages can be incorporated using:

- [Microsoft Visual Studio](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio)
- [Microsoft Visual Studio Code](https://code.visualstudio.com/docs/csharp/package-management)
- [JetBrains Rider](https://www.jetbrains.com/help/rider/Using_NuGet.html)
- or any IDE [paired with the Microsoft dotnet command line interface](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-dotnet-cli). 

> [!NOTE]
> [Templates for .NET](/nservicebus/dotnet-templates/) are available to make it easier to get started creating NServiceBus endpoints.

## ServiceControl and ServicePulse

While developing NServiceBus endpoints the other Particular Platform tools can be useful for visualizing, debugging, and testing the functionality of a messaging-based system.

### Running in Containers

ServiceControl and ServicePulse can be run in a container hosting. [Examples are available](https://github.com/particular/PlatformContainerExamples) to help you get started running those tools.

### Running locally

ServiceControl and ServicePulse deployed locally through [downloads located on the particular.net website](https://particular.net/downloads).

## ServiceInsight

ServiceInsight must be [downloaded](https://particular.net/downloads) and installed locally.
