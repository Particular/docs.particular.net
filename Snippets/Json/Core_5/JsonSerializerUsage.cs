﻿namespace Core5
{
    using System.Text;
    using NServiceBus;

    class JsonSerializerUsage
    {
        JsonSerializerUsage(BusConfiguration busConfiguration)
        {
            #region JsonSerialization

            busConfiguration.UseSerialization<JsonSerializer>();

            #endregion

            #region JsonSerializationEncoding

            var noBomEncoding = new UTF8Encoding(false, false);

            busConfiguration.UseSerialization<JsonSerializer>()
                .Encoding(noBomEncoding);

            #endregion
        }


    }

}