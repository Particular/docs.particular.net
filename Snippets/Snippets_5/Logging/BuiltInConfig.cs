using NServiceBus.Logging;

public class BuiltInConfig
{
    public void ChangingDefaults()
    {
        string pathToLoggingDirectory = null;
        // start code OverrideLoggingDefaultsInCode
        LogManager.ConfigureDefaults(LogLevel.Debug, pathToLoggingDirectory);
        // end code OverrideLoggingDefaultsInCode
    }
}