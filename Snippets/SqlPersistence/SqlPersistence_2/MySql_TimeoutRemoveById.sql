startcode MySql_TimeoutRemoveByIdSql

set @sagaId := (select SagaId from `EndpointNameTimeoutData` where Id = @Id);
delete from `EndpointNameTimeoutData`
where Id = @Id;
select @sagaId;
endcode
