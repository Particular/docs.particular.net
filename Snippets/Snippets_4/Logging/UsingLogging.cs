namespace Snippets4.Logging
{
    using NServiceBus.Logging;

    #region UsingLogging

    public class ClassUsingLogging
    {
        static ILog logger = LogManager.GetLogger(typeof(ClassUsingLogging));

        public void SomeMethod()
        {
            //your code
            logger.Debug("Something interesting happened.");
        }

    }

    #endregion

}