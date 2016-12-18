startcode MsSqlServer_SagadGetByPropertySql

select
    Id,
    Metadata,
    Data,
    SagaTypeVersion,
    SagaVersion
from EndpointNameSagaName
where Correlation_PropertyName = @propertyValue

endcode
