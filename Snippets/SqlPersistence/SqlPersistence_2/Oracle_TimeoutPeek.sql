startcode Oracle_TimeoutPeekSql

select
    Destination,
    SagaId,
    State,
    ExpireTime,
    Headers
from EndpointNameTO
where Id = :Id
endcode
