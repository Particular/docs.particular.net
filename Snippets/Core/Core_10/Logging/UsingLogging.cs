namespace Core.Logging;

using NServiceBus.Logging;

#region UsingLogging

public class ClassUsingLogging
{
    static ILog log = LogManager.GetLogger<ClassUsingLogging>();
    readonly int times = 2;

    public void SomeMethod()
    {
        log.Warn("Something unexpected happened.");
        if (log.IsDebugEnabled)
        {
            log.Debug("Something expected happened.");
            log.DebugFormat("Also, this other thing happened {0} times.", times);
        }
    }
}

#endregion