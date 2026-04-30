namespace Core.Logging;

using NServiceBus.Logging;

class BuiltInConfig
{
#pragma warning disable CS0618 // Type or member is obsolete

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
#pragma warning restore CS0618 // Type or member is obsolete
}