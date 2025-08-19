startcode Oracle_SagaGetByPropertySql

select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from EndpointName_SagaName
where CORR_PROPERTYNAME = :propertyValue
for update

endcode
