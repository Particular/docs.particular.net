startcode MsSqlServer_SagaGetBySagaIdSql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointNameSagaName
where Id = @Id

endcode
