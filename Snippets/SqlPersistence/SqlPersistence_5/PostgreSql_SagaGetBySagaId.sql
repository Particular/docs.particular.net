startcode PostgreSql_SagaGetBySagaIdSql

select
    "Id",
    "SagaTypeVersion",
    "Concurrency",
    "Metadata",
    "Data"
from EndpointName_SagaName
where "Id" = @Id
for update

endcode
