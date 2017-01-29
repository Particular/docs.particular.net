startcode MySql_SagaCompleteSql

delete from EndpointNameSagaName
where Id = @Id AND Concurrency = @Concurrency

endcode
