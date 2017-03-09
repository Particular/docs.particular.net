startcode MySql_TimeoutPeekSql

select
    Destination,
    SagaId,
    State,
    Time,
    Headers
from `EndpointNameTimeoutData`
where Id = @Id
endcode
