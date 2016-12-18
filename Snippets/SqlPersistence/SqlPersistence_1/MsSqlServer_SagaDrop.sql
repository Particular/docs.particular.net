startcode MsSqlServer_SagaDropSql

/* TableNameVariable */

declare @tableName nvarchar(max) = @tablePrefix + 'OrderSaga';

/* DropTable */

if exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName)
        and type in ('U')
)
begin
    declare @dropTable nvarchar(max);
    set @dropTable = 'drop table ' + @tableName;
    exec(@dropTable);
end

endcode
