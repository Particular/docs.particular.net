startcode MsSqlServer_SubscriptionCreateSql
declare @tableName nvarchar(max) = @tablePrefix + 'SubscriptionData';
if not exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName) and
        type in (N'U')
)
begin
declare @createTable nvarchar(max);
set @createTable = N'
    create table ' + @tableName + '(
        Subscriber nvarchar(450) not null,
        Endpoint nvarchar(450) null,
        MessageType nvarchar(450) not null,
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
