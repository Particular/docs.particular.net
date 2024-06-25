﻿namespace Core9.DataBus.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningDataBusPropertiesAs(
                property => property.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}