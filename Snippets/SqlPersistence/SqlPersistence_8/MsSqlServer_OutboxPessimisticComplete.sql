startcode MsSqlServer_OutboxPessimisticCompleteSql

update [dbo].[EndpointNameOutboxData]
set
    Operations = @Operations
where MessageId = @MessageId
endcode
