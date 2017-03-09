startcode MsSqlServer_TimeoutRemoveByIdSql

delete from [dbo].[EndpointNameTimeoutData]
output deleted.SagaId
where Id = @Id
endcode
