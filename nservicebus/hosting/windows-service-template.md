---
title: Windows Service 'dotnet new' Template
reviewed: 2017-06-01
component: ServiceTemplate
related:
 - servicecontrol/transport-adapter/template
tags:
- Hosting
---

The NServiceBus `dotnet new` template makes it easier to create a Windows Service host for NServiceBus.


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