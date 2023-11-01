startcode PostgreSql_TimeoutAddSql

insert into "public"."EndpointNameTimeoutData"
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
    @Time,
    @Headers,
    @PersistenceVersion
)
endcode
