using System;
using System.Linq;
using NServiceBus;

#region 6to7-ProfileForLogging

class RuntimeProfile :
    IConfigureThisEndpoint
{
    public void Customize(BusConfiguration busConfiguration)
    {
        var profile = Environment.GetCommandLineArgs();

        if (profile.Contains("Production"))
        {
            // configure the production profile
        }
    }
}

#endregion
