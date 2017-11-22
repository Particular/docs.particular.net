startcode PostgreSql_SagaSaveSql

insert into EndpointName_SagaName
(
    "Id",
    "Metadata",
    "Data",
    "PersistenceVersion",
    "SagaTypeVersion",
    "Concurrency",
    "Correlation_CorrelationProperty",
    "Correlation_TransitionalCorrelationProperty"
)
values
(
    @Id,
    @Metadata,
    @Data,
    @PersistenceVersion,
    @SagaTypeVersion,
    1,
    @CorrelationId,
    @TransitionalCorrelationId
)
endcode
