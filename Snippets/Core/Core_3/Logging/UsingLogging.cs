namespace Core3.Logging
{
    using log4net;

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
