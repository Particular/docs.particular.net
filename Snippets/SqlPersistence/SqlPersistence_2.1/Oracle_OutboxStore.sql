startcode Oracle_OutboxStoreSql

insert into "ENDPOINTNAMEOD"
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
