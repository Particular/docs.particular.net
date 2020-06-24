startcode Oracle_OutboxSetAsDispatchedSql

update "ENDPOINTNAMEOD"
set
    Dispatched = 1,
    DispatchedAt = :DispatchedAt,
    Operations = '[]'
where MessageId = :MessageId
endcode
