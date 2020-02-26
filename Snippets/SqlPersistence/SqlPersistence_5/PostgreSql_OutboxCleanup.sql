startcode PostgreSql_OutboxCleanupSql

delete from "public"."EndpointNameOutboxData"
where ctid in
(
    select ctid
    from "public"."EndpointNameOutboxData"
    where
        "Dispatched" = true and
        "DispatchedAt" < @DispatchedBefore
    limit @BatchSize
)
endcode
