startcode MySql_SagadGetByPropertySql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointNameSagaName
where Correlation_PropertyName = @propertyValue

endcode
