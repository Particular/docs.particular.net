---
title: Installer Workflow
component: SqlPersistence
related:
 - nservicebus/operations
 - samples/sql-persistence
 - persistence/sql/install
reviewed: 2019-09-03
redirects:
 - nservicebus/sql-persistence/installer-workflow
---


{{WARNING:

The following pages should be reviewed prior to this page:

 * [SQL Persistence NuGet Packages](/persistence/sql/#nuget-packages)
 * [Installation and deployment](/persistence/sql/install.md)
}}


## Contrasting Workflows

[NServiceBus Installers](/nservicebus/operations/installers.md) offer an automatic deployment model which requires elevated database permissions to correctly execute. In non-development environments least priviledged access or operational deployment control is often required, making the use of automatic SQL installer scripts forbidden. To streamline development execution of installers can be enabled only on developers machines. This kind of behavior can be enabled with the following code:

snippet: InstallerWorkflow

In this example the `IsDevelopment` environment variable is used to enable script execution on development machines. A development machine could also be identified by checking a machine name convention, using some command line argument or incorporate another toggle mechanism that will be environment-specific.

In higher environments (especially in production) the installation should be designed as an integral part of the deployment process. In particular, it is recommended to put in place mechanisms to verify generated scripts and test them before running in production. It may also be necessary to customize certain settings (e.g. schemas) or to modify the scripts themselves for every environment.

Depending on the organization and the choice of tooling, the process can be fully automated, partially automated or even fully manual.

### Development Workflow

The main purpose in this type of environment is having the "Just hit F5" experience, by automatically creating required tables in a local database (or remote database used only for development). The risk and potential impact of accidentally corrupting the database schema or existing data is minimal in this environment.

A typical workflow on developers machine consists of the following steps:

 1. In the project, add references to the [necessary NuGet package(s)](/persistence/sql/#nuget-packages).
 2. Build the project.
 3. During build, SQL installation scripts are automatically created in the location appropriate for the Visual Studio project type, e.g. `bin\Debug\NServiceBus.Persistence.Sql` or `bin\Debug\{target}\NServiceBus.Persistence.Sql`.
 4. Start the solution in Visual Studio.
 5. The NServiceBus endpoint starts and executes its configuration code, including checking for the toggle enabling installer execution.
 6. If enabled, SQL installation scripts are executed and the required tables are created.



### Higher Environment Workflow

NOTE: "Higher Environment" is a general term to refer to any non-development environment. For example "Integration" or "Production".

The workflow in a higher environment will differ based on the specifics of an organizations process. For example, it's possible to allow endpoints [to automatically execute scripts](/persistence/sql/install.md#script-execution-in-non-development-environments) or [to take full control and execute them using custom code](/persistence/sql/install.md#script-execution-in-non-development-environments).

In case of taking full control of script execution, it is necessary to plug-in the script execution into whatever deployment pipeline the organization has in place, for example:

 * When using Octopus deploy, the scripts would be executed in a ["Step"]( https://octopus.com/docs/deploying-applications/adding-steps).
 * When using TeamCity, the scripts would be executed in a ["Build Step"]( https://confluence.jetbrains.com/display/TCD8/Configuring+Build+Steps).

The generated scripts do not contain all variables, because some of them are gathered only at runtime, e.g. `@schema` and `@tablePrefix`. Those variables need to be explicitly provided when scripts are executed. Installers automatically gather that information from endpoint configuration (e.g. by default the endpoint name is used as a `@tablePrefix` value).

Additionally, scripts assume the required databases and schemas already exist. Even when executed automatically by installers, scripts don't contain statements creating those elements, as they require additional configuration, such as appropriate security settings, taking into account redundancy and backup plans, etc. 

A sample workflow in that kind of environment can consist of the following steps:

 1. In the project, add references to the [necessary NuGet package(s)](/persistence/sql/#nuget-packages).
 1. Build Project.
 1. Scripts are created in the output directory.
 1. Scripts are copied to a deployment package along with the output assemblies.
 1. Scripts are reviewed by a DBA or QA team and approved.
 1. Scripts are executed in higher environment as part of the existing deployment pipeline. Required settings, such as schemas or table prefixes, have different values in each environment. They are passed to scripts together with other configuration settings, such as connection string.
 1. The endpoint is deployed and started.