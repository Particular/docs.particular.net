startcode MySql_SagaSaveSql

insert into EndpointNameSagaName
(
    Id,
    Metadata,
    Data,
    PersistenceVersion,
    SagaTypeVersion,
    Concurrency,
    Correlation_CorrelationProperty,
    Correlation_TransitionalCorrelationProperty
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
