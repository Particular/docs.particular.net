
- Oracle 12.2 and below supports [max. 30 characters](https://docs.oracle.com/database/121/SQLRF/sql_elements008.htm#SQLRF00223), see the [Oracle Caveats](/persistence/sql/oracle-caveats.md) article to learn more.
- MySQL supports [max. 64 characters](https://dev.mysql.com/doc/refman/5.7/en/identifiers.html).
- SQL Server supports [max. 128 characters](https://docs.microsoft.com/en-us/sql/sql-server/maximum-capacity-specifications-for-sql-server).

Note: In case of Oracle SQL persistence will throw an exception in case the name length is too long. See the [Oracle caveats](/persistence/sql/oracle-caveats.md)  In case of database engines other than Oracle, the SQL persistence will not validate name length, for two reasons. Firstly, the supported name length value is higher and should be sufficient for typical scenarios. Secondly, it is possible to modify the setting locally to support longer names. In case of long names for sagas, etc. the database engine may perform automatic name truncation.
