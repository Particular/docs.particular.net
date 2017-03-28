startcode MySql_OutboxSetAsDispatchedSql

update `EndpointNameOutboxData`
set
    Dispatched = 1,
    DispatchedAt = @DispatchedAt,
    Operations = '[]'
where MessageId = @MessageId
endcode
