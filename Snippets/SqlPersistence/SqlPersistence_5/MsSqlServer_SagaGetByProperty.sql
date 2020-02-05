startcode MsSqlServer_SagaGetByPropertySql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointName_SagaName
with (updlock)
where Correlation_PropertyName = @propertyValue

endcode
