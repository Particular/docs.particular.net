startcode Oracle_OutboxGetSql

select
    Dispatched,
    Operations
from "ENDPOINTNAMEOD"
where MessageId = :MessageId
endcode
