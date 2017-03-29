startcode MySql_OutboxCleanupSql

delete from EndpointNameOutboxData where Dispatched = 1 And DispatchedAt < @Date
endcode
