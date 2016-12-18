startcode MsSqlServer_SagaGetBySagaIdSql

select
    Id,
    Metadata,
    Data,
    SagaTypeVersion,
    SagaVersion
from EndpointNameSagaName
where Id = @Id

endcode
