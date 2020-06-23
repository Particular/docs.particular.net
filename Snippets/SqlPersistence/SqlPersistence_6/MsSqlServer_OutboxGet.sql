startcode MsSqlServer_OutboxGetSql

select
    Dispatched,
    Operations
from [dbo].[EndpointNameOutboxData]
where MessageId = @MessageId
endcode
