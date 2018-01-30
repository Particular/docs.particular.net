Starting from version 2.2.2, there is a non-clustered index on the `[Expires]` column. This index speeds up the purging of expired messages from the queue table. If the SQL Server transport discovers that a required index is missing, it logs an appropriate warning. The following SQL statement can be used to create the missing index:

snippet: sql-ExpiresIndex