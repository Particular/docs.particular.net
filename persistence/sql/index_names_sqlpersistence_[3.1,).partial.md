
 * Oracle 12.2 and below supports [max. 30 characters](https://docs.oracle.com/database/121/SQLRF/sql_elements008.htm#SQLRF00223), see the [Oracle Caveats](/persistence/sql/oracle-caveats.md) article to learn more.
 * MySQL supports [max. 64 characters](https://dev.mysql.com/doc/refman/5.7/en/identifiers.html).
 * SQL Server supports [max. 128 characters](https://docs.microsoft.com/en-us/sql/sql-server/maximum-capacity-specifications-for-sql-server).
 * PostgreSQL supports [max. 63 characters](https://www.postgresql.org/docs/current/static/sql-syntax-lexical.html#sql-syntax-identifiers).

Note: In case of Oracle and PostgreSQL, the SQL persistence will throw an exception in case the name length is too long. See the [Oracle caveats](/persistence/sql/oracle-caveats.md) or [PostgreSQL caveats](/persistence/sql/postgresql-design.md) to learn more. In case of MySQL and MS SQL Server, the SQL persistence will not validate name length, for two reasons. Firstly, the supported name length values are higher and should be sufficient for typical scenarios. Secondly, it is possible to modify the database setting locally to support longer names. In case of long names for sagas, etc. the database engine may perform automatic name truncation.
