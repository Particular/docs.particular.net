startcode MsSqlServer_SagaCompleteSql

delete from EndpointNameSagaName
where Id = @Id AND Concurrency = @Concurrency

endcode
