startcode MySql_SagaUpdateSql

update EndpointNameSagaName
set
    Data = @Data,
    PersistenceVersion = @PersistenceVersion,
    SagaTypeVersion = @SagaTypeVersion,
    Concurrency = @Concurrency + 1,
    Correlation_TransitionalCorrelationPproperty = @TransitionalCorrelationId
where
    Id = @Id AND Concurrency = @Concurrency

endcode
