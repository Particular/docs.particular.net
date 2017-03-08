startcode MsSqlServer_SagaUpdateSql

update EndpointNameSagaName
set
    Data = @Data,
    PersistenceVersion = @PersistenceVersion,
    SagaTypeVersion = @SagaTypeVersion,
    Concurrency = @Concurrency + 1,
    Correlation_TransitionalCorrelationProperty = @TransitionalCorrelationId
where
    Id = @Id AND Concurrency = @Concurrency

endcode
