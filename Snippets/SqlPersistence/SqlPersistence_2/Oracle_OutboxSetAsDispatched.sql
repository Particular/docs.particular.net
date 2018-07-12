startcode Oracle_OutboxSetAsDispatchedSql

update "dbo"."ENDPOINTNAMEOD"
set
    Dispatched = 1,
    DispatchedAt = :DispatchedAt,
    Operations = '[]'
where MessageId = :MessageId
endcode
