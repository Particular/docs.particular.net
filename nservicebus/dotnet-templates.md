---
title: dotnet new Templates
reviewed: 2020-08-03
component: Templates
related:
 - servicecontrol/transport-adapter
 - nservicebus/hosting/windows-service
 - samples/servicecontrol/adapter-mixed-transports
redirects:
 - nservicebus/hosting/windows-service-template
 - servicecontrol/transport-adapter/template
---

The Particular [dotnet new](https://docs.microsoft.com/dotnet/core/tools/dotnet-new) templates makes it easier to bootstrap a variety of common project and code related scenarios.

NOTE: The `dotnet new` command creates a new project, configuration file, or solution based on the specified template. The command provides a convenient way to initialize a valid SDK-style project. The command calls the template engine to create the artifacts on disk based on the specified template and options. More information is available in the [dotnet-new documentation](https://docs.microsoft.com/dotnet/core/tools/dotnet-new).

## Installation

Install using the following command:

snippet: install


## NServiceBus Windows Service

This template makes it easier to create a [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) host for an NServiceBus endpoint.

The template can be used via the following command:

snippet: nsbservice-usage

This will create a new directory named `MyWindowsService` containing a Windows Service `.csproj` also named `MyWindowsService`.

To add to an existing solution:

snippet: nsbservice-addToSolution


### Options

snippet: nsbservice-options

partial: target-framework

NOTE: When installing an endpoint created from this template as a service, the `--run-as-service` parameter must be set on the command line. See [Windows Service Installation](/nservicebus/hosting/windows-service.md) for details.


## ServiceControl Transport Adapter Windows Service

This template makes it easier to create a [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) host for a [ServiceControl Transport Adapter](/servicecontrol/transport-adapter/).

The template can be used via the following command:

snippet: scadapterservice-usage

This will create a new directory named `MyAdapter` containing a windows service `.csproj` also named `MyAdapter`.

To add to an existing solution:

snippet: scadapterservice-addToSolution


### Options

snippet: scadapterservice-options

partial: target-framework

NOTE: When installing an endpoint created from this template as a service, the `--run-as-service` parameter must be set on the command line. See [Windows Service Installation](/nservicebus/hosting/windows-service.md) for details.


partial: dockercontainer
