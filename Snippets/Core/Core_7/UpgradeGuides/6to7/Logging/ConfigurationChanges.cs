namespace Core_7.UpgradeGuides._6to7.Logging
{
    using System;
    using System.Configuration;
    using NServiceBus.Logging;

    public class ConfigurationChanges
    {

        public void Level()
        {
            #region 6to7LoggingCode
            var logFactory = LogManager.Use<DefaultFactory>();
            logFactory.Level(LogLevel.Info);
            #endregion
        }
        public void AppConfig()
        {
            #region 6to7LoggingReadAppSettings
            var logFactory = LogManager.Use<DefaultFactory>();
            var level = (LogLevel)Enum.Parse(typeof(LogLevel), ConfigurationManager.AppSettings.Get("LoggingThreshold"));
            logFactory.Level(level);
            #endregion
        }
    }

}