﻿using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

public static class Connections
{
    static string ConnectionString(string catalog) => $@"Data Source=.\SQLEXPRESS;Initial Catalog={catalog};Integrated Security=True";

    static string Sales => ConnectionString("sales");
    static string Shipping => ConnectionString("shipping");
    static string Adapter => ConnectionString("adapter");

    public static async Task<SqlConnection> GetConnection(string destination)
    {
        var connectionString = GetConnectionString(destination);

        var connection = new SqlConnection(connectionString);

        await connection.OpenAsync()
            .ConfigureAwait(false);

        return connection;
    }

    #region GetConnectionString

    static readonly string[] AdapterQueues = {"audit", "error", "poison", "Particular.ServiceControl"};

    static string GetConnectionString(string destination)
    {
        if (destination.StartsWith("Samples.ServiceControl.SqlServerTransportAdapter.Sales", StringComparison.OrdinalIgnoreCase))
        {
            return Sales;
        }
        if (destination.StartsWith("Samples.ServiceControl.SqlServerTransportAdapter.Shipping", StringComparison.OrdinalIgnoreCase))
        {
            return Shipping;
        }
        if (AdapterQueues.Any(q => string.Equals(q, destination, StringComparison.OrdinalIgnoreCase)))
        {
            return Adapter;
        }
        throw new Exception($"Unknown destination: {destination}");
    }

    #endregion
}