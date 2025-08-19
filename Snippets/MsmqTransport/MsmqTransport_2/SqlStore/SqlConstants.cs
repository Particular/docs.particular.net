static class SqlConstants
{
    #region dms-sql-crud
    public const string SqlInsert = "INSERT INTO {0} (Id, Destination, Time, Headers, State) VALUES (@id, @destination, @time, @headers, @state);";
    public const string SqlFetch = "SELECT TOP 1 Id, Destination, Time, Headers, State, RetryCount FROM {0} WITH (READPAST, UPDLOCK, ROWLOCK) WHERE Time < @time ORDER BY Time";
    public const string SqlDelete = "DELETE {0} WHERE Id = @id";
    public const string SqlUpdate = "UPDATE {0} SET RetryCount = RetryCount + 1 WHERE Id = @id";
    public const string SqlGetNext = "SELECT TOP 1 Time FROM {0} ORDER BY Time";
    #endregion
    #region dms-sql-create-table
    public const string SqlCreateTable = @"
if not exists (
    select * from sys.objects
    where
        object_id = object_id('{0}')
        and type in ('U')
)
begin
    create table {0} (
 	    Id nvarchar(250) not null primary key,
        Destination nvarchar(200),
        State varbinary(max),
        Time datetime,
        Headers varbinary(max) not null,
        RetryCount INT NOT NULL default(0)
        )
end

if not exists
(
    select *
    from sys.indexes
    where
        name = 'Index_Time' and
        object_id = object_id('{0}')
)
begin
    create index Index_Time on {0} (Time);
end
";
    #endregion
}