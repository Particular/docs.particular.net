startcode Oracle_OutboxStoreSql

insert into "dbo"."ENDPOINTNAMEOD"
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
