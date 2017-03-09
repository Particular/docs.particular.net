startcode MsSqlServer_TimeoutDropSql
declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'TimeoutData]';

if exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName) and
        type in ('U')
)
begin
declare @dropTable nvarchar(max);
set @dropTable = 'drop table ' + @tableName;
exec(@dropTable);
end
endcode
