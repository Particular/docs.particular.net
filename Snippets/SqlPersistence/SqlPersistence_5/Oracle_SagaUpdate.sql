startcode Oracle_SagaUpdateSql

update EndpointName_SagaName
set
    Data = :Data,
    PersistenceVersion = :PersistenceVersion,
    SagaTypeVersion = :SagaTypeVersion,
    Concurrency = :Concurrency + 1,
    CORR_TRANSITIONALCORRELATIONPR = :TransitionalCorrelationId
where
    Id = :Id and Concurrency = :Concurrency

endcode
