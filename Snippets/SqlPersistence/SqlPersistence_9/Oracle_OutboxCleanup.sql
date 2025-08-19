startcode Oracle_OutboxCleanupSql

delete from "ENDPOINTNAMEOD"
where Dispatched = 1 and
      DispatchedAt < :DispatchedBefore and
      rownum <= :BatchSize
endcode
