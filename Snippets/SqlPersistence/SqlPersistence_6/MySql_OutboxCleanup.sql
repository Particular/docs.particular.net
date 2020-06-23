startcode MySql_OutboxCleanupSql

delete from `EndpointNameOutboxData`
where Dispatched = true and
      DispatchedAt < @DispatchedBefore
limit @BatchSize
endcode
