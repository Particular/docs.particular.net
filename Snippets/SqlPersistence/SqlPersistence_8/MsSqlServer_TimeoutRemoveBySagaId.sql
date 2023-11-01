startcode MsSqlServer_TimeoutRemoveBySagaIdSql

delete from [dbo].[EndpointNameTimeoutData]
where SagaId = @SagaId
endcode
