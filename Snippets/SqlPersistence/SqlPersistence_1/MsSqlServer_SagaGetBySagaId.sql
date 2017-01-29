startcode MsSqlServer_SagaGetBySagaIdSql

select
    Id,
    Metadata,
    Data,
    SagaTypeVersion,
    Concurrency
from EndpointNameSagaName
where Id = @Id

endcode
