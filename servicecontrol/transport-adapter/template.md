---
title: ServiceControl Transport Adapter Template
summary: ServiceControl Transport Adapter dotnet new Template
component: SCTransportAdapterTemplate
reviewed: 2017-06-28
related:
 - nservicebus/hosting/windows-service-template
---

The Transport Adapter `dotnet new` template makes it easier to create a [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) host for the Transport Adapter.


## Installation

Install using the following:

snippet: install


## Usage

The template can then be used via the following.

snippet: usage

This will create a new directory named `MyAdapter` containing a windows service `.csproj` also named `MyAdapter`.

To add to an existing solution:

snippet: addToSolution

include: dotnetnew
