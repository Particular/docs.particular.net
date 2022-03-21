using Microsoft.Data.SqlClient;

#region ConnectionHolder
public class ConnectionHolder
{
    public SqlConnection Connection { get; set; }
    public SqlTransaction Transaction { get; set; }
}
#endregion