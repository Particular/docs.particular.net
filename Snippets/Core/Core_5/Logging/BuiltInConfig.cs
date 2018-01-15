namespace Core5.Logging
{
    using NServiceBus.Logging;

    class BuiltInConfig
    {

        void ChangingDefaults()
        {

            #region OverrideLoggingDefaultsInCode

            var defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Level(LogLevel.Debug);
            defaultFactory.Directory("pathToLoggingDirectory");

            #endregion
        }
        void ChangingLevel()
        {

            #region OverrideLoggingLevelInCode

            var defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Level(LogLevel.Debug);

            #endregion
        }

    }
}
