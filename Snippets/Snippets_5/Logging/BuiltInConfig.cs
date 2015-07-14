namespace Snippets5.Logging
{
    using NServiceBus.Logging;

    public class BuiltInConfig
    {

        public void ChangingDefaults()
        {

            #region OverrideLoggingDefaultsInCode

            DefaultFactory defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Directory("pathToLoggingDirectory");
            defaultFactory.Level(LogLevel.Debug);

            #endregion
        }
        public void ChangingLevel()
        {

            #region OverrideLoggingLevelInCode

            DefaultFactory defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Level(LogLevel.Debug);

            #endregion
        }

    }
}