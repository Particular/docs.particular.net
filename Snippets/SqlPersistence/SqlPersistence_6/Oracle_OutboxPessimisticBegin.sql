startcode Oracle_OutboxPessimisticBeginSql

insert into "ENDPOINTNAMEOD"
(
    MessageId,
    Operations,
    PersistenceVersion
)
values
(
    :MessageId,
    '[]',
    :PersistenceVersion
)
endcode
