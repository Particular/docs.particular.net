
public static class Connections
{
    public static string ReceiverConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesStoreAndForwardReceiver;Integrated Security=True;Max Pool Size=100";
    public static string SenderConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesStoreAndForwardSender;Integrated Security=True;Max Pool Size=100";

    static Connections()
    {
        SqlHelper.EnsureDatabaseExists(SenderConnectionString);
        SqlHelper.EnsureDatabaseExists(ReceiverConnectionString);
    }
}