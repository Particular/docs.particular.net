startcode Oracle_OutboxGetSql

select
    Dispatched,
    Operations
from "dbo"."ENDPOINTNAMEOD"
where MessageId = :MessageId
endcode
