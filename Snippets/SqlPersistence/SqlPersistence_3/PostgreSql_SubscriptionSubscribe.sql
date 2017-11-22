startcode PostgreSql_SubscriptionSubscribeSql

insert into "public"."EndpointNameSubscriptionData"
(
    "Id",
    "Subscriber",
    "MessageType",
    "Endpoint",
    "PersistenceVersion"
)
values
(
    concat(@Subscriber, @MessageType),
    @Subscriber,
    @MessageType,
    @Endpoint,
    @PersistenceVersion
)
on conflict ("Id") do update
    set "Endpoint" = @Endpoint,
        "PersistenceVersion" = @PersistenceVersion

endcode
