startcode MySql_OutboxCleanupSql

delete from EndpointNameOutboxData where Dispatched = true And DispatchedAt < @Date
endcode
