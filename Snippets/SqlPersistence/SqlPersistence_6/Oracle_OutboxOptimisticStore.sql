startcode Oracle_OutboxOptimisticStoreSql

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
