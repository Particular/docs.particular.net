using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameV4

        //With a string
        Configure.With()
            .DefineEndpointName("MyEndpoint");

        //With a Func
        Configure.With()
            .DefineEndpointName(() => "MyEndpoint");

        #endregion
    }

}