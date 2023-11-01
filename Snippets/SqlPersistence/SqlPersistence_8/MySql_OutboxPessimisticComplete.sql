startcode MySql_OutboxPessimisticCompleteSql

update `EndpointNameOutboxData`
set
    Operations = @Operations
where MessageId = @MessageId
endcode
