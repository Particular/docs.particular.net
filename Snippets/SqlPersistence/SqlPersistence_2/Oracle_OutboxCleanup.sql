startcode Oracle_OutboxCleanupSql

delete from "dbo"."ENDPOINTNAMEOD"
where Dispatched = 1
    and DispatchedAt < :DispatchedBefore
    and rownum <= :BatchSize
endcode
