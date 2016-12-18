startcode MySql_TimeouSelectByIdSql

select
    Destination,
    SagaId,
    State,
    Time,
    Headers
from EndpointNameTimeoutData
where Id = @Id
endcode
