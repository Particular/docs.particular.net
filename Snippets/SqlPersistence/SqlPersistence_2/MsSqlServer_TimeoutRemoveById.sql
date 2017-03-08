startcode MsSqlServer_TimeoutRemoveByIdSql

delete from EndpointNameTimeoutData
output deleted.SagaId
where Id = @Id
endcode
