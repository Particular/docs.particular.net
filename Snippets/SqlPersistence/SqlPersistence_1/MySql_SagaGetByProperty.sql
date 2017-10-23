startcode MySql_SagaGetByPropertySql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointNameSagaName
where Correlation_PropertyName = @propertyValue

endcode
