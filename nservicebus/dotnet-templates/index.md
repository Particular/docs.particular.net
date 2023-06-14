---
title: dotnet new Templates
reviewed: 2023-06-14
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

partial: nsbendpoint
partial: nsbhandler
partial: nsbsaga

partial: winservice
partial: sc-transport-adapter
partial: dockercontainer
