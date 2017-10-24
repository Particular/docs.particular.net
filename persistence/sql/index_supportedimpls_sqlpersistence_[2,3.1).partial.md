
 * [SQL Server](https://www.microsoft.com/en-au/sql-server/) ([Version 2012](https://docs.microsoft.com/en-us/sql/release-notes/sql-server-2012-release-notes) and above due to the use of the [THROW functionality](https://docs.microsoft.com/en-us/sql/t-sql/language-elements/throw-transact-sql)).
 * [MySQL](https://www.mysql.com/) ([Version 5.7](https://dev.mysql.com/doc/relnotes/mysql/5.7/en/) and above due to the use of [JSON Data Type](https://dev.mysql.com/doc/refman/5.7/en/json.html)).
 * [Oracle](https://www.oracle.com/database/index.html) ([Version 11g Release 2](https://docs.oracle.com/cd/E11882_01/readmes.112/e41331/chapter11204.htm) and above).
   * Due to the 30-character limit on table, key, and index names, there are some [Oracle-specific caveats to consider](oracle-caveats.md).

WARNING: This persistence will run on the free version of the above engines, i.e. [SQL Server Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express), [MySQL Community Edition](https://www.mysql.com/products/community/), [Oracle XE](http://www.oracle.com/technetwork/database/database-technologies/express-edition/overview/index.html). However it is strongly recommended to use commercial versions for any production system. It is also recommended to ensure that support agreements are in place from the software vendor or another third party support provider. For example:

 * [Microsoft Premier Support](https://www.microsoft.com/en-us/microsoftservices/support.aspx)
 * [MySQL support](https://www.mysql.com/support/)
 * [Oracle Support](https://www.oracle.com/support/index.html)