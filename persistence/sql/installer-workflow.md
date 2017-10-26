---
title: Installer Workflow
component: SqlPersistence
tags:
 - Persistence
related:
 - nservicebus/operations
 - samples/sql-persistence
reviewed: 2016-11-29
redirects:
 - nservicebus/sql-persistence/installer-workflow
---

WARNING: Read about [SQL Persistence NuGet Packages](/persistence/sql/#nuget-packages) before proceeding.


## Contrasting Workflows

Generally SQL installer script execution at endpoint startup will only be desired on a developers machine.

So the code to enable this would be something similar to.

snippet: InstallerWorkflow

In this manner a developers machine can be identified by the existence of the `IsDevelopment` environment variable. A development machine could also be identified by checking a machine name convention, command line argument or any other toggle based on the specific environment.


### Development Workflow

A standard development work operates as follows:

 1. Build Project.
 1. `NServiceBus.Persistence.Sql.MsBuild` creates scripts in `bin\Debug\NServiceBus.Persistence.Sql`.
 1. Solution is started in Visual Studio.
 1. Endpoint starts.
 1. Toggle is checked and identified as a development machine.
 1. SQL installer script are executed and the required persistence tables are created.


### Higher Environment Workflow

The workflow in higher environment may differ based on the specifics of an organizations process. As such the below is a possible workflow.

 1. Build Project.
 1. `NServiceBus.Persistence.Sql.MsBuild` creates scripts in `bin\Debug\NServiceBus.Persistence.Sql`.
 1. Scripts are copied to a deployment package along with the output assemblies.
 1. Scripts are reviewed by a DBA or QA team and approved.
 1. Scripts are executed in higher environment.
 1. Endpoint is deployed and started.
