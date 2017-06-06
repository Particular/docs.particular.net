---
title: ServiceControl Transport Adapter Template
summary: ServiceControl Transport Adapter dotnet new Template
component: SCTransportAdapterTemplate
related:
 - nservicebus/hosting/windows-service-template
---

The Transport Adapter Template `dotnet new` template makes it easier to create a [Windows Service](https://msdn.microsoft.com/en-us/library/d56de412.aspx) host for the Transport Adapter.


## Installation

Install using the following:

snippet: install


## Usage

The template can then be used via the following.

snippet: usage

This will create a new directory named `MyWindowsService` containing a windows service `.csproj` also named `MyWindowsService`.

To add to an existing solution:

snippet: addToSolution

include: dotnetnew