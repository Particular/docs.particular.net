using Microsoft.Data.SqlClient;
using System;

public class ConnectionHolder
{
    public SqlConnection Connection { get; set; }
    public SqlTransaction Transaction { get; set; }

    public Guid Id { get; } = Guid.NewGuid();

    public override string ToString()
    {
        return $"{Id}: HasConn: {Connection != null}, HasTx: {Transaction != null}";
    }
}