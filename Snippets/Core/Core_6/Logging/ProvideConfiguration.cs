﻿#pragma warning disable 618
namespace Core6.Logging
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region LoggingThresholdFromIProvideConfiguration

    public class ProvideConfiguration :
        IProvideConfiguration<Logging>
    {
        public Logging GetConfiguration()
        {
            return new Logging
            {
                Threshold = "Info"
            };
        }
    }

    #endregion
}