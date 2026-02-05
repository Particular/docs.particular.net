---
title: Operational Scripting
summary: How to generate database scripts for the SQL persister
component: SqlPersistence
reviewed: 2026-01-02
---

In order to generate database scripts outside of a build process, use the `NServiceBus.Persistence.Sql.CommandLine` command prompt tool developed for this purpose.

The tool can be downloaded from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.Persistence.Sql.CommandLine
```

Once installed, the `sql-persistence` command line tool will be available for use.

`sql-persistence <command> [options]`

### Available commands
This tool currently has one command.
- `script`

### sql-persistence script

Generates scripts required to setup a database for an NServiceBus endpoint:

```
sql-persistence script <assembly>
                              [--output-dir]
                              [--clean]
                              [--overwrite]
                              [--dialect]
                              [--verbose]
```

#### Options
 
`-o=<path>` | `--output-dir=<path>` : Path to the output directory. If not specified, the current directory will be used.

`--clean` : Removes existing files in the output directory

`--overwrite`: Overwrites existing files in the output if they match the files to be generated

`--dialect=<value>`: Specifies a dialect to generate. Allowed values are: SqlServer, MySql, Oracle, PostgreSql

`--verbose`: Verbose logging
