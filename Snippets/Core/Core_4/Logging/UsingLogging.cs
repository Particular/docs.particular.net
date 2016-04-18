namespace Core4.Logging
{
    using NServiceBus.Logging;

    #region UsingLogging

    public class ClassUsingLogging
    {
        static ILog logger = LogManager.GetLogger(typeof(ClassUsingLogging));

        public void SomeMethod()
        {
            logger.Debug("Something interesting happened.");
        }

    }

    #endregion

}