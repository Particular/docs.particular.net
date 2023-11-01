startcode MySql_OutboxGetSql

select
    Dispatched,
    Operations
from `EndpointNameOutboxData`
where MessageId = @MessageId
endcode
