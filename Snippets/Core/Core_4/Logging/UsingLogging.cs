namespace Core4.Logging
{
    using NServiceBus.Logging;

    #region UsingLogging

    public class ClassUsingLogging
    {
        static ILog log = LogManager.GetLogger(typeof(ClassUsingLogging));

        public void SomeMethod()
        {
            log.Debug("Something interesting happened.");
        }

    }

    #endregion

}