startcode Oracle_SagaGetBySagaIdSql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from "ENDPOINTNAME_SAGANAME"
where Id = :Id

endcode
