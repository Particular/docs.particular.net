startcode MsSqlServer_SagadGetByPropertySql

select
    Id,
    Metadata,
    Data,
    SagaTypeVersion,
    Concurrency
from EndpointNameSagaName
where Correlation_PropertyName = @propertyValue

endcode
