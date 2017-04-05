startcode Oracle_OutboxStoreSql

insert into EndpointNameOD
(
    MessageId,
    Operations,
    PersistenceVersion
)
values
(
    :MessageId,
    :Operations,
    :PersistenceVersion
)
endcode
