---
title: Controlling script generation
summary: Control how SQL scripts are generated for the SQL persister
component: SqlPersistence
related:
 - persistence/sql/operational-scripting
reviewed: 2019-10-04
---


SQL installation scripts are created as compile-time outputs alongside a project's binary outputs. These scripts can be promoted to a directory under source control so that differences can be tracked and analyzed.

partial: scriptlocation

For example, for a project named `ClassLibrary` built in Debug mode, the following directories will be created.

 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MsSqlServer`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MySql`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\Oracle`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\PostgreSql`

Scripts will also be included in the list of project output files. This means that those files produced will be copied to the output directory of any project that references it.

WARNING: Projects using `project.json` are **not** supported. The `project.json` approach was an experiment by Microsoft for a new project system that was not based on MSBuild. Since `project.json` did not support running MSBuild files shipped inside a NuGet, the SQL Persistence script creation does not work. This experiment has since been abandoned. To fix this, either migrate back to the older Visual Studio 2015 project format (`.csproj` and `packages.config`) or migrate to the new [Visual Studio 2017 project format](https://docs.microsoft.com/en-us/dotnet/core/tools/project-json-to-csproj). [dotnet-migrate](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-migrate) can help migrating to the new `.csproj` format.

partial: DisableGeneration

## SqlPersistenceSettings attribute

Scripts creation is configured via `[SqlPersistenceSettings]` applied to the target assembly.


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
