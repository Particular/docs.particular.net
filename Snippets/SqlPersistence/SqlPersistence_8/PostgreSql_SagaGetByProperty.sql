startcode PostgreSql_SagaGetByPropertySql

select
    "Id",
    "SagaTypeVersion",
    "Concurrency",
    "Metadata",
    "Data"
from EndpointName_SagaName
where "Correlation_PropertyName" = @propertyValue
for update

endcode
