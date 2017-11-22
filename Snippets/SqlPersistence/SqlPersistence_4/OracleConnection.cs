using System;
using System.Data;
using System.Data.Common;

class OracleConnection : DbConnection
{
    public OracleConnection(string connection)
    {

    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        throw new NotImplementedException();
    }

    public override void Close()
    {
    }

    public override void ChangeDatabase(string databaseName)
    {
    }

    public override void Open()
    {
    }

    public override string ConnectionString { get; set; }
    public override string Database { get; }
    public override ConnectionState State { get; }
    public override string DataSource { get; }
    public override string ServerVersion { get; }

    protected override DbCommand CreateDbCommand()
    {
        throw new NotImplementedException();
    }
}