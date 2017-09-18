namespace Host_8.UpgradeGuides
{
    using NServiceBus;

    class EndpointSLA
    {
        //todo: Where do I put the code for endpoint SLA?
#pragma warning disable 618
        #region 7to8EndpointSLABefore

        //[EndpointSLA("00:00:10")]
        public class MyEndpoint:IConfigureThisEndpoint
        {
            public void Customize(EndpointConfiguration configuration)
            {
                
            }
        }

        #endregion
#pragma warning restore 618
    }
}