using NServiceBus;

#region CustomHostLogging
class MyEndpointConfig :
    IConfigureThisEndpoint,
    IWantCustomLogging
{
    public void Init()
    {
        // setup logging infrastructure
        SetLoggingLibrary.Log4Net(null, new MyLogger());
    }
}
#endregion

class MyLogger
{
}