﻿using NServiceBus;
using NServiceBus.Logging;

#region CustomCustomizeConfiguration
public class CustomizeConfiguration :
    ICustomizeConfiguration
{
    static ILog log = LogManager.GetLogger<CustomizeConfiguration>();

    public void Run(BusConfiguration busConfiguration)
    {
        log.Info("Setting serializer to XML in an extension");
        busConfiguration.UseSerialization<XmlSerializer>();
    }
}
#endregion