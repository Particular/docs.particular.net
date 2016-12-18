startcode MsSqlServer_SagaSaveSql

insert into EndpointNameSagaName
(
    Id,
    Metadata,
    Data,
    PersistenceVersion,
    SagaTypeVersion,
    SagaVersion,
Correlation_CorrelationPproperty,
Correlation_TransitionalCorrelationPproperty
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
