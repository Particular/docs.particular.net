## Custom SQL Server transport connection factory

In some environments it might be necessary to adapt to database server settings, or to perform additional operations. For example, if the `NOCOUNT` setting is enabled on the server, then it is necessary to send the `SET NOCOUNT OFF` command right after opening the connection.

That can be done by passing the transport a custom factory method which will provide connection strings at runtime, and which can perform custom actions:

snippet: sqlserver-custom-connection-factory

NOTE: If opening the connection fails, the custom connection factory must dispose the connection object and rethrow the exception.
