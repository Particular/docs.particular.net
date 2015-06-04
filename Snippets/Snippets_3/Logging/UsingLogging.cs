namespace Snippets3.Logging
{
    using log4net;

    #region UsingLogging

    public class ClassUsingLogging
    {
        public void SomeMethod()
        {
            //your code
            Logger.Debug("Something interesting happened.");
        }

        static ILog Logger = LogManager.GetLogger("Name");
    }

    #endregion
}
