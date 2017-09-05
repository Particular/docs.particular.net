namespace Host_8.UpgradeGuides
{
    using NServiceBus;

    class EndpointSLA
    {
        #region 7to8EndpointSLABefore

        [EndpointSLA("00:00:10")]
        public class MyEndpoint:IConfigureThisEndpoint
        {
            public void Customize(EndpointConfiguration configuration)
            {
                
            }
        }

        #endregion
    }
}