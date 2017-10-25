startcode PostgreSql_TimeoutRemoveBySagaIdSql

delete from "public"."EndpointNameTimeoutData"
where "SagaId" = @SagaId
endcode
