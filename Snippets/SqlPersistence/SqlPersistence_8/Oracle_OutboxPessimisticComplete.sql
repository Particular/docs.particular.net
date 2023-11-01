startcode Oracle_OutboxPessimisticCompleteSql

update "ENDPOINTNAMEOD"
set
    Operations = :Operations
where MessageId = :MessageId
endcode
