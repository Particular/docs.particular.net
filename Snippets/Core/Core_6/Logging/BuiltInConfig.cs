namespace Core6.Logging
{
    using NServiceBus.Logging;

    class BuiltInConfig
    {

        void ChangingDefaults()
        {
            #region OverrideLoggingDefaultsInCode

            var defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Directory("pathToLoggingDirectory");
            defaultFactory.Level(LogLevel.Debug);

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