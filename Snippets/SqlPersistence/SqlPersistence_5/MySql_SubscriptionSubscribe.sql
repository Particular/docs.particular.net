startcode MySql_SubscriptionSubscribeSql

insert into `EndpointNameSubscriptionData`
(
    Subscriber,
    MessageType,
    Endpoint,
    PersistenceVersion
)
values
(
    @Subscriber,
    @MessageType,
    @Endpoint,
    @PersistenceVersion
)
on duplicate key update
    Endpoint = coalesce(@Endpoint, Endpoint),
    PersistenceVersion = @PersistenceVersion

endcode
