﻿using Newtonsoft.Json;
using NServiceBus;

class XmlUsage
{
    void UseConverter(EndpointConfiguration endpointConfiguration)
    {
        #region UseConverter

        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Converters =
            {
                new XmlJsonConverter()
            }
        };

        var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        serialization.Settings(settings);

        #endregion
    }
}