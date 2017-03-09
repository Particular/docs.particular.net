startcode MsSqlServer_OutboxCleanupSql

delete from [dbo].[EndpointNameOutboxData] where Dispatched = true And DispatchedAt < @Date
endcode
