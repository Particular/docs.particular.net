#region UsingLogging
using log4net;

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
