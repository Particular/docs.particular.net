startcode Oracle_SagaSaveSql

insert into "ENDPOINTNAME_SAGANAME"
(
    Id,
    Metadata,
    Data,
    PersistenceVersion,
    SagaTypeVersion,
    Concurrency,
    CORR_CORRELATIONPROPERTY,
    CORR_TRANSITIONALCORRELATIONPR
)
values
(
    :Id,
    :Metadata,
    :Data,
    :PersistenceVersion,
    :SagaTypeVersion,
    1,
    :CorrelationId,
    :TransitionalCorrelationId
)
endcode
