startcode Oracle_OutboxCleanupSql

delete from EndpointNameOD
where Dispatched = 1
    and DispatchedAt < :DispatchedBefore
    and rownum <= :BatchSize
endcode
