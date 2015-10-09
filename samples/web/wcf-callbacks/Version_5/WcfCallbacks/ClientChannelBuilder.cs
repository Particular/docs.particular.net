using System.ServiceModel;

public static class ClientChannelBuilder
{

    public static ChannelFactory<ICallbackService<TMessage, TResponse>> GetChannelFactory<TMessage, TResponse>(string server)
    {
        var myBinding = new BasicHttpBinding();
        string address = AddressBuilder.GetAddress<TMessage, TResponse>(server);
        var myEndpoint = new EndpointAddress(address);
        return new ChannelFactory<ICallbackService<TMessage, TResponse>>(myBinding, myEndpoint);
    }
}