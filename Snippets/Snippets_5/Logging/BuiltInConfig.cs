using NServiceBus.Logging;

public class BuiltInConfig
{
    public void ChangingDefaults()
    {
        var pathToLoggingDirectory = "";

        #region OverrideLoggingDefaultsInCode

        LogManager.Use<DefaultFactory>(d =>
        {
            d.LoggingDirectory = pathToLoggingDirectory;
            d.LogLevel = LogLevel.Debug;
        });

        #endregion
    }
}