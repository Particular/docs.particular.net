namespace Snippets5.Logging
{
    using NServiceBus.Logging;

    class BuiltInConfig
    {

        void ChangingDefaults()
        {

            #region OverrideLoggingDefaultsInCode

            DefaultFactory defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Directory("pathToLoggingDirectory");
            defaultFactory.Level(LogLevel.Debug);

            #endregion
        }
        void ChangingLevel()
        {

            #region OverrideLoggingLevelInCode

            DefaultFactory defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Level(LogLevel.Debug);

            #endregion
        }

    }
}