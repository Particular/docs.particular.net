startcode PostgreSql_OutboxPessimisticCompleteSql

update "public"."EndpointNameOutboxData"
set
    "Operations" = @Operations
where "MessageId" = @MessageId
endcode
