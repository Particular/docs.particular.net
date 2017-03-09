startcode MySql_SagadGetByPropertySql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointName_SagaName
where Correlation_PropertyName = @propertyValue

endcode
