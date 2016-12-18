startcode MsSqlServer_SagaUpdateSql

update EndpointNameSagaName
set
    Data = @Data,
    PersistenceVersion = @PersistenceVersion,
    SagaTypeVersion = @SagaTypeVersion,
    SagaVersion = @SagaVersion + 1,
Correlation_TransitionalCorrelationPproperty = @TransitionalCorrelationId
where
    Id = @Id AND SagaVersion = @SagaVersion

endcode
