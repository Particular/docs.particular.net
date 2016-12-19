---
title: Installer Workflow
component: SqlPersistence
tags:
 - Persistence
related:
 - samples/sql-persistence
reviewed: 2016-11-29
---

## Packages

The Sql Persistence consists of several Nuget Packages


### Runtime and static configuration

The [NServiceBus.Persistence.Sql NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql/) serves several purposes.

 * Provides the runtime functionality for persisting all [storage types](/nservicebus/persistence/#storage-types) 
 * Provides static configuration (in the form of attributes and types) that allow settings to be passed consumed by the `NServiceBus.Persistence.Sql.MsBuild` NuGet package at compile time.
 * Optionally runs SQL installation scripts at endpoint startup.


### MsBuild Script Creation
 
The [NServiceBus.Persistence.Sql.MsBuild NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild/) creates all the SQL installation scripts. It does this by wiring into the MSBuild pipeline and interrogating types and attributes to infer what scripts to create. This package has a dependency on the `NServiceBus.Persistence.Sql` NuGet package


### Script Builder

The [NServiceBus.Persistence.Sql.ScriptBuilder NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.ScriptBuilder/) is a class library that provides an API for creating all SQL installation scripts. This library is used internally by the `NServiceBus.Persistence.Sql.MsBuild` NuGet package at compile time.


## Contrasting Workflows

Generally sql installer script execution at endpoint startup will only be desired on a developers machine.

So the code to enable this would be something similar to.

snippet: InstallerWorkflow

In this manner a developers machine can be identified by the existence of the `IsDevelopement` environment variable. A development machine could also be identified by checking a machine name convention, command line argument or any other toggle based on the specific environment.


### Development Workflow

A standard development work operates as follows:

 1. Build Project.
 1. `NServiceBus.Persistence.Sql.MsBuild` creates scripts in `bin\Debug\NServiceBus.Persistence.Sql`.
 1. Solution is started in Visual Studio.
 1. Endpoint starts.
 1. Toggle is checked and identified as a development machine.
 1. Sql installer script are executed and the required persistence tables are created.


### Higher Environment Workflow

The workflow in higher environment may differ based on the specifics of an organizations process. As such the below is a possible work.

 1. Build Project.
 1. `NServiceBus.Persistence.Sql.MsBuild` creates scripts in `bin\Debug\NServiceBus.Persistence.Sql`.
 1. Sql installer scripts are copied to a deployment package along with the output assemblies.
 1. Sql installer scripts are reviewed by a DBA or QA team and approved.
 1. Sql installer scripts are executed in higher environment.
 1. Endpoint is deployed and started.