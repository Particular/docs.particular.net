public static class AddressBuilder
{

    public static string GetAddress<TMessage, TResponse>(string server)
    {
        return string.Format("{2}/BusService/{0}_{1}", typeof(TMessage).Name, typeof(TResponse).Name, server);
    }
}