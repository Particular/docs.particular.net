---
title: MySQL dialect
component: SqlPersistence
related:
reviewed: 2017-11-23
---

{{WARNING: This persistence will run on the free version of the above engines, i.e. [MySQL Community Edition](https://www.mysql.com/products/community/). However it is strongly recommended to use commercial versions for any production system. It is also recommended to ensure that support agreements are in place. See [MySQL support](https://www.mysql.com/support/) for details.
}}


## Supported database versions

SQL persistence supports [MySQL 5.7](https://dev.mysql.com/doc/relnotes/mysql/5.7/en/) and above. It does not support lower versions due to the use of [JSON Data Type](https://dev.mysql.com/doc/refman/5.7/en/json.html).

## Usage

include: usage

Using the [MySql.Data NuGet Package](https://www.nuget.org/packages/MySql.Data/).

snippet: SqlPersistenceUsageMySql

{{Note: The following settings are required for [MySQL connections string](https://dev.mysql.com/doc/connector-net/en/connector-net-connection-options.html).

 * `AllowUserVariables=True`: since the Persistence uses [user variables](https://dev.mysql.com/doc/refman/5.7/en/user-variables.html).
 * `AutoEnlist=false`: To prevent auto enlistment in a [Distributed Transaction](https://msdn.microsoft.com/en-us/library/windows/desktop/ms681205.aspx) which the MySql .NET connector does not currently support.}}


## Unicode support

include: unicode-support

Refer to the dedicated [MySQL documentation](https://dev.mysql.com/doc/refman/5.7/en/charset-applications.html) for details.


## Supported name lengths

include: name-lengths

MySQL supports [max. 64 characters](https://dev.mysql.com/doc/refman/5.7/en/identifiers.html).

include: name-length-validation-off

