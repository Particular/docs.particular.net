namespace Snippets3.UpgradeGuides._3to4
{
    using NServiceBus;

    public class Upgrade
    {
        public void DisableSecondLevelRetries()
        {
            #region 3to4DisableSecondLevelRetries
            Configure configure = Configure.With();
            configure.DisableSecondLevelRetries();
            #endregion
        }

        public void EnableSagas()
        {
            #region 3to4EnableSagas
            Configure configure = Configure.With();
            configure.Sagas();
            #endregion
        }

    }
}