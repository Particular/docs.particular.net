startcode Oracle_TimeoutAddSql

insert into EndpointNameTO
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
