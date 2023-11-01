startcode Oracle_TimeoutPeekSql

select
    Destination,
    SagaId,
    State,
    ExpireTime,
    Headers
from "ENDPOINTNAMETO"
where Id = :Id
endcode
