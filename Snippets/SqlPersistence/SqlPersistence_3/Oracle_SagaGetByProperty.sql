startcode Oracle_SagaGetByPropertySql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from "ENDPOINTNAME_SAGANAME"
where CORR_PROPERTYNAME = :propertyValue

endcode
