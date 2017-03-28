### Indexes

Each queue table has a clustered index on the `[RowVersion]` column in order to speed up receiving messages from the queue table.

Starting from version 2.2.2, each queue table also has an additional non-clustered index on the `[Expires]` column. This index speeds up the purging of expired messages from the queue table. If the SQL Server transport discovers that a required index is missing, it will log an appropriate warning. The following SQL statement can be used to create the missing index:

snippet: sql-ExpiresIndex