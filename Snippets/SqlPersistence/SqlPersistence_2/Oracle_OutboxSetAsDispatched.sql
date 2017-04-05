startcode Oracle_OutboxSetAsDispatchedSql

update EndpointNameOD
set
    Dispatched = 1,
    DispatchedAt = :DispatchedAt,
    Operations = '[]'
where MessageId = :MessageId
endcode
