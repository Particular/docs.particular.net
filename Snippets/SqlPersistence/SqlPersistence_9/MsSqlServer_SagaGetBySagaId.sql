startcode MsSqlServer_SagaGetBySagaIdSql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointName_SagaName
with (updlock)
where Id = @Id

endcode
