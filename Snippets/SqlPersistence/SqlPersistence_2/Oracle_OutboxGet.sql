startcode Oracle_OutboxGetSql

select
    Dispatched,
    Operations
from EndpointNameOD
where MessageId = :MessageId
endcode
