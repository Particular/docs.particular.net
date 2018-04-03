---
title: Controlling script generation
component: SqlPersistence
reviewed: 2018-01-02
---


SQL installation scripts are created at compile time by the `NServiceBus.Persistence.Sql.MsBuild` NuGet package.

partial: scriptlocation

For example for a project named `ClassLibrary` built in Debug mode the following directories will be created.

 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MsSqlServer`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MySql`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\Oracle`

Scripts will also be included in the list of project output files. This means those files produced will be copied to the output directory of any project that references it.

Scripts creation can configured via `[SqlPersistenceSettings]` applied to the target assembly.

WARNING: Projects using `project.json` are **not** supported. The `project.json` approach was an experiment by Microsoft at a new project system that was not based on MSBuild. Since `project.json` did not support running MSBuild files shipped inside a NuGet the SQL Persistence script creation does not work. This experiment has since been abandoned. To fix this either migrate back to the old Visual Studio 2015 project format (`.csproj` and `packages.config`) or migrate to the new [Visual Studio 2017 project format](https://docs.microsoft.com/en-us/dotnet/core/tools/project-json-to-csproj). [dotnet-migrate](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-migrate) can help migrating to the new `.csproj` format.

### To produce all scripts

snippet: AllSqlScripts


### To produce only MS SQL Server scripts

snippet: SqlServerScripts


### To produce only MySQL scripts

snippet: MySqlScripts

partial: scriptsoracle

partial: scriptspostgresql


partial: scripttoggle

partial: promote
