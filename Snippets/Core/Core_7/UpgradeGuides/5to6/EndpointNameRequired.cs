﻿namespace Core6.UpgradeGuides._5to6
{
    using NServiceBus;
    
    class EndpointNameRequired
    {
        EndpointNameRequired()
        {
            #region 5to6-endpointNameRequired
            var endpointConfiguration = new EndpointConfiguration("MyEndpointName");
            #endregion
        }

    }

}
