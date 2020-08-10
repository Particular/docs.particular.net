namespace Core8.Logging
{
    using NServiceBus.Logging;

    class BuiltInConfig
    {

        void ChangingLevel()
        {
            #region OverrideLoggingLevelInCode

            var defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Level(LogLevel.Debug);

            #endregion
        }

        void ChangingDirectory()
        {

            #region OverrideLoggingDirectoryInCode

            var defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Directory("pathToLoggingDirectory");

            #endregion
        }

    }
}
