public static class AddressBuilder
{

    public static string GetAddress<TMessage, TResponse>(string server)
    {
        return $"{server}/BusService/{typeof(TMessage).Name}_{typeof(TResponse).Name}";
    }
}