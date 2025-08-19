startcode MsSqlServer_SagaSelectSql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointName_SagaName
with (updlock)
where 1 = 1

endcode
