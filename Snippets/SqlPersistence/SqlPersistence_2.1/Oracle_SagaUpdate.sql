startcode Oracle_SagaUpdateSql

update "ENDPOINTNAME_SAGANAME"
set
    Data = :Data,
    PersistenceVersion = :PersistenceVersion,
    SagaTypeVersion = :SagaTypeVersion,
    Concurrency = :Concurrency + 1,
    CORR_TRANSITIONALCORRELATIONPR = :TransitionalCorrelationId
where
    Id = :Id and Concurrency = :Concurrency

endcode
