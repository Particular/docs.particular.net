startcode MsSqlServer_TimeoutRemoveBySagaIdSql

delete from EndpointNameTimeoutData
where SagaId = @SagaId
endcode
