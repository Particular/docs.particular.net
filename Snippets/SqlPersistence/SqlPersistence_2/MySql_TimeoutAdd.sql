startcode MySql_TimeoutAddSql

insert into `EndpointNameTimeoutData`
(
    Id,
    Destination,
    SagaId,
    State,
    Time,
    Headers,
    PersistenceVersion
)
values
(
    @Id,
    @Destination,
    @SagaId,
    @State,
    @Time,
    @Headers,
    @PersistenceVersion
)
endcode
