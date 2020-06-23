startcode MySql_SagaSelectSql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointName_SagaName
where 1 = 1
for update

endcode
