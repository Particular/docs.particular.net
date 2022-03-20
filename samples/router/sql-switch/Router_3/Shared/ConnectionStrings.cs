using System;

public class ConnectionStrings
{
    public const string Blue = @"Data Source=(local);Initial Catalog=sqlswitch_blue;Integrated Security=True;Max Pool Size=100";
    //public const string red = @"Data Source=.\SQLEXPRESS;Initial Catalog=sqlswitch_red;Integrated Security=True;Max Pool Size=100";
    //public const string green = @"Data Source=.\SQLEXPRESS;Initial Catalog=sqlswitch_green;Integrated Security=True;Max Pool Size=100";

    public static string Red
        => Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");

    public static string Green
        => Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
}