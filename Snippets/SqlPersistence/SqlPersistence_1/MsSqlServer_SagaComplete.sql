startcode MsSqlServer_SagaCompleteSql

delete from EndpointNameSagaName
where Id = @Id AND SagaVersion = @SagaVersion

endcode
