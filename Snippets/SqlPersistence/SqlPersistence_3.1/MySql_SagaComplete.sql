startcode MySql_SagaCompleteSql

delete from EndpointName_SagaName
where Id = @Id and Concurrency = @Concurrency

endcode
