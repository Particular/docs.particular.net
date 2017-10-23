startcode MsSqlServer_SagaUpdateSql

update EndpointName_SagaName
set
    Data = @Data,
    PersistenceVersion = @PersistenceVersion,
    SagaTypeVersion = @SagaTypeVersion,
    Concurrency = @Concurrency + 1,
    Correlation_TransitionalCorrelationProperty = @TransitionalCorrelationId
where
    Id = @Id and Concurrency = @Concurrency

endcode
