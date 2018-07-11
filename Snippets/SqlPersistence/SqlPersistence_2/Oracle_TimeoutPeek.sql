startcode Oracle_TimeoutPeekSql

select
    Destination,
    SagaId,
    State,
    ExpireTime,
    Headers
from "dbo"."ENDPOINTNAMETO"
where Id = :Id
endcode
