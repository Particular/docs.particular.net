startcode MsSqlServer_SubscriptionCreateSql
declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + 'SubscriptionData]';

if not exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName) and
        type in ('U')
)
begin
declare @createTable nvarchar(max);
set @createTable = '
    create table ' + @tableName + '(
        Subscriber nvarchar(200) not null,
        Endpoint nvarchar(200),
        MessageType nvarchar(200) not null,
        PersistenceVersion varchar(23) not null,
        primary key clustered
        (
            Subscriber,
            MessageType
        )
    )
';
exec(@createTable);
end
endcode
