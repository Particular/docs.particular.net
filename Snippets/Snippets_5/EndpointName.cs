using NServiceBus;

public class EndpointName
{
    public void Simple()
    {
        #region EndpointNameV5

        //With a string
        Configure.With(b => b.EndpointName("MyEndpoint"));

        #endregion
    }

}