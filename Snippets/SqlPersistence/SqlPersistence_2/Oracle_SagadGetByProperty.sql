startcode Oracle_SagadGetByPropertySql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointName_SagaName
where CORR_PROPERTYNAME = :propertyValue

endcode
