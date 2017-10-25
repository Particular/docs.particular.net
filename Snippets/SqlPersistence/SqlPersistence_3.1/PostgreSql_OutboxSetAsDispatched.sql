startcode PostgreSql_OutboxSetAsDispatchedSql

update "public"."EndpointNameOutboxData"
set
    "Dispatched" = true,
    "DispatchedAt" = @DispatchedAt,
    "Operations" = '[]'
where "MessageId" = @MessageId
endcode
