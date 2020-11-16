using System;
using Microsoft.Data.SqlClient;
using NServiceBus;

#region TruncateTableAtStartup

public class TruncateTableAtStartup : INeedInitialization
{
    public void Customize(EndpointConfiguration configuration)
    {
        var connectionString = @"Data Source=.\SqlExpress;Database=SQLServerTruncate;Integrated Security=True;Max Pool Size=100";

        SqlHelper.TruncateMessageTable(connectionString, "Samples.SqlServer.TruncateReceiver");
    }
}

#endregion