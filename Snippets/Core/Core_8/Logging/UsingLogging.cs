namespace Core8.Logging
{
    using NServiceBus.Logging;

    #region UsingLogging

    public class ClassUsingLogging
    {
        static ILog log = LogManager.GetLogger<ClassUsingLogging>();

        public void SomeMethod()
        {
            log.Warn("Something unexpected happened.");
            if (log.IsDebugEnabled)
            {
                log.Debug("Something expected happened.");
            }
        }
    }

    #endregion
}
