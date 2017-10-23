startcode PostgreSql_OutboxSetAsDispatchedSql

update public."EndpointNameOutboxData"
set
    "Dispatched" = true,
    "DispatchedAt" = @DispatchedAt at time zone 'UTC',
    "Operations" = '[]'
where "MessageId" = @MessageId
endcode
