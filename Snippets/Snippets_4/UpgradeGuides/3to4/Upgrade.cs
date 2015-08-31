namespace Snippets4.UpgradeGuides._3to4
{
    using System.Transactions;
    using NServiceBus;

    public class Upgrade
    {
        public void RevertToSerializable()
        {
            #region 3to4RevertToSerializable
            Configure.Transactions.Advanced(settings =>
               settings.IsolationLevel(IsolationLevel.Serializable));
            #endregion
        }
        
        public void DisableSecondLevelRetries()
        {
            #region 3to4DisableSecondLevelRetries
            Configure.Features.Disable<NServiceBus.Features.SecondLevelRetries>();
            #endregion
        }

        public void EnableSagas()
        {
            #region 3to4EnableSagas
            Configure.Features.Enable<NServiceBus.Features.Sagas>();
            #endregion
        }

    }
}