startcode PostgreSql_TimeoutAddSql

insert into public."EndpointNameTimeoutData"
(
    "Id",
    "Destination",
    "SagaId",
    "State",
    "Time",
    "Headers",
    "PersistenceVersion"
)
values
(
    @Id,
    @Destination,
    @SagaId,
    @State,
    @Time at time zone 'UTC',
    @Headers,
    @PersistenceVersion
)
endcode
