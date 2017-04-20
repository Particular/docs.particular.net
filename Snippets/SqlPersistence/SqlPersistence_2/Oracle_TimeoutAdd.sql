startcode Oracle_TimeoutAddSql

insert into "ENDPOINTNAMETO"
(
    Id,
    Destination,
    SagaId,
    State,
    ExpireTime,
    Headers,
    PersistenceVersion
)
values
(
    :Id,
    :Destination,
    :SagaId,
    :State,
    :Time,
    :Headers,
    :PersistenceVersion
)
endcode
