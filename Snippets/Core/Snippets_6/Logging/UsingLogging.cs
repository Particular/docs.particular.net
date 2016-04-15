namespace Core6.Logging
{
    using NServiceBus.Logging;

    #region UsingLogging

    public class ClassUsingLogging
    {
        static ILog logger = LogManager.GetLogger<ClassUsingLogging>();

        public void SomeMethod()
        {
            logger.Debug("Something interesting happened.");
        }
    }

    #endregion
}
