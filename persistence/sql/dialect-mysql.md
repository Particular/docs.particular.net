---
title: PostgreSQL dialect design
component: SqlPersistence
related:
reviewed: 2017-11-23
redirects:
 - nservicebus/sql-persistence/postgresql-design
---

## Unicode support

include: unicode-support

Refer to the dedicated [MySQL documentation](https://dev.mysql.com/doc/refman/5.7/en/charset-applications.html) for details.

## Supported name lengths

MySQL supports [max. 64 characters](https://dev.mysql.com/doc/refman/5.7/en/identifiers.html).

include: name-length-validation-off

## Usage

include: usage

Using the [MySql.Data NuGet Package](https://www.nuget.org/packages/MySql.Data/).

snippet: SqlPersistenceUsageMySql

{{Note: The following settings are required for [MySQL connections string](https://dev.mysql.com/doc/connector-net/en/connector-net-connection-options.html).

 * `AllowUserVariables=True`: since the Persistence uses [user variables](https://dev.mysql.com/doc/refman/5.7/en/user-variables.html).
 * `AutoEnlist=false`: To prevent auto enlistment in a [Distributed Transaction](https://msdn.microsoft.com/en-us/library/windows/desktop/ms681205.aspx) which the MySql .NET connector does not currently support.}}