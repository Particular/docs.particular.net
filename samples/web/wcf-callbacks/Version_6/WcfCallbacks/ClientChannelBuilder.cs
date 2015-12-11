using System.ServiceModel;

#region ClientChannelBuilder
public static class ClientChannelBuilder
{
    public static ChannelFactory<ICallbackService<TMessage, TResponse>> GetChannelFactory<TMessage, TResponse>(string server)
    {
        BasicHttpBinding myBinding = new BasicHttpBinding();
        string address = AddressBuilder.GetAddress<TMessage, TResponse>(server);
        EndpointAddress myEndpoint = new EndpointAddress(address);
        return new ChannelFactory<ICallbackService<TMessage, TResponse>>(myBinding, myEndpoint);
    }
}
#endregion