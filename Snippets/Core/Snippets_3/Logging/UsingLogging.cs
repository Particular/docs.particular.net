namespace Core3.Logging
{
    using log4net;

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
