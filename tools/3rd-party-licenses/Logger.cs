using NuGet.Common;

public class Logger : ILogger
{
    public void LogDebug(string data) => Log(LogLevel.Debug, data);

    public void LogVerbose(string data) => Log(LogLevel.Verbose, data);

    public void LogInformation(string data) => Log(LogLevel.Information, data);

    public void LogMinimal(string data) => Log(LogLevel.Minimal, data);

    public void LogWarning(string data) => Log(LogLevel.Warning, data);

    public void LogError(string data) => Log(LogLevel.Error, data);

    public void LogInformationSummary(string data) => Log(LogLevel.Information, data);

    public void LogErrorSummary(string data) => Log(LogLevel.Error, data);

    public void Log(LogLevel level, string data)
    {
        switch (level)
        {
            case LogLevel.Minimal:
            case LogLevel.Warning:
            case LogLevel.Error:
                Console.WriteLine($"{level}:{data}");
                break;
            default:
                break;
        }
    }

    public Task LogAsync(LogLevel level, string data)
    {
        Log(level, data);
        return Task.CompletedTask;
    }

    public void Log(ILogMessage message) => Log(message.Level, message.Message);

    public Task LogAsync(ILogMessage message) => LogAsync(message.Level, message.Message);
}