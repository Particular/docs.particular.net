startcode MySql_OutboxCleanupSql

delete from `EndpointNameOutboxData`
where Dispatched = true
    and DispatchedAt < @Date
limit @BatchSize
endcode
