using NServiceBus.Logging;

public class BuiltInConfig
{
    public void ChangingDefaults()
    {
        var pathToLoggingDirectory = "";

        #region OverrideLoggingDefaultsInCode

        LogManager.ConfigureDefaults(LogLevel.Debug, pathToLoggingDirectory);

        #endregion
    }
}