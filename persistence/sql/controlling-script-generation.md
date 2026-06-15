---
title: Controlling script generation
summary: Control how SQL scripts are generated for the SQL persister
component: SqlPersistence
related:
 - persistence/sql/operational-scripting
reviewed: 2026-06-15
---


SQL installation scripts are created as compile-time outputs alongside a project's binary outputs. These scripts can be promoted to a directory under source control so that differences can be tracked and analyzed.

partial: scriptlocation

For example, for a project named `ClassLibrary` built in Debug mode, the following directories will be created.

 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MsSqlServer`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\MySql`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\Oracle`
 * `ClassLibrary\bin\Debug\NServiceBus.Persistence.Sql\PostgreSql`

Scripts will also be included in the list of project output files. This means that those files produced will be copied to the output directory of any project that references the assembly.

partial: DisableGeneration

## SqlPersistenceSettings attribute

Script creation is configured via `[SqlPersistenceSettings]` applied to the target assembly.


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
