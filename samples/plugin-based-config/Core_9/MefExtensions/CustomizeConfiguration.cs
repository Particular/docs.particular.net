﻿using System.Composition;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region MefCustomizeConfiguration
[Export(typeof(ICustomizeConfiguration))]
public class CustomizeConfiguration :
    ICustomizeConfiguration
{
    static ILog log = LogManager.GetLogger<CustomizeConfiguration>();

    public Task Run(EndpointConfiguration endpointConfiguration)
    {
        log.Info("Setting serializer to Xml in an extension");
        endpointConfiguration.UseSerialization<XmlSerializer>();
        return Task.CompletedTask;
    }
}
#endregion