The SQL Persister consists of several [NuGet packages](https://www.nuget.org/packages?q=NServiceBus.Persistence.Sql).


### [NServiceBus.Persistence.Sql.MsBuild](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild/)

This packages adds to the [MSBuild](https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild) pipeline and generates the required SQL installation scripts at compile time. It does this by interrogating types (in the target assembly) and attributes (from the `NServiceBus.Persistence.Sql` NuGet package) to infer what scripts to create. It is required for any project where those SQL installation scripts are required. For saga scripts, this is any project that contains saga classes. For timeouts, subscriptions, and outbox scripts, it is the endpoint hosting project. This package has a dependency on the `NServiceBus.Persistence.Sql` NuGet package.


### [NServiceBus.Persistence.Sql](https://www.nuget.org/packages/NServiceBus.Persistence.Sql/)

This package contains several parts:

 * APIs for manipulating `EndpointConfiguration`.
 * Runtime implementations of saga, timeouts, subscriptions, and outbox persisters.
 * Attributes used to define compile-time configuration settings. These attributes are interrogated by the `NServiceBus.Persistence.Sql.MsBuild` NuGet Package when generating SQL installation scripts.
 * Optionally runs SQL installation scripts at endpoint startup for development purposes. See [Installer Workflow](installer-workflow.md).


### [NServiceBus.Persistence.Sql.ScriptBuilder](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.ScriptBuilder/)

This package contains the APIs that enable the generation of SQL installation scripts using code, i.e. without using the `NServiceBus.Persistence.Sql.MsBuild` NuGet package.

DANGER: `NServiceBus.Persistence.Sql.ScriptBuilder` is not ready for general usage. It was made public and deployed to NuGet primarily to enable the generation of documentation in a repeatable way. For example, it is used to generate the SQL scripts in the [MS SQL Server Scripts](/persistence/sql/sqlserver-scripts.md), [MySql Scripts](/persistence/sql/mysql-scripts.md), [Oracle Scripts](/persistence/sql/oracle-scripts.md), and [PostgreSQL Scripts](/persistence/sql/postgresql-scripts.md) pages. In future releases, the API may evolve in ways that do not follow [semantic versioning](/nservicebus/upgrades/release-policy.md#semantic-versioning). This can be discussed in more detail in the [Particular Software discussion forum](https://discuss.particular.net/).