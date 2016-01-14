namespace Snippets4.UpgradeGuides._3to4
{
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Unicast.Config;

    public class Upgrade
    {

        public void RenamePrincipalHack()
        {
            #region 3to4RenamePrincipalHack
            Configure configure = Configure.With();
            ConfigUnicastBus unicastBus = configure.UnicastBus();
            unicastBus.RunHandlersUnderIncomingPrincipal(true);
            #endregion
        }

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

        public void RunDistributor()
        {
            #region 3to4RunDistributor

            Configure configure = Configure.With();
            configure.RunMSMQDistributor();

            #endregion
        }
        public void EnlistWithDistributor()
        {
            #region 3to4EnlistWithDistributor

            Configure configure = Configure.With();
            configure.EnlistWithMSMQDistributor();

            #endregion
        }

        public void SetMessageHeader()
        {
            IBus bus = null;

            #region 3to4SetMessageHeader

            MyMessage myMessage = new MyMessage();
            bus.SetMessageHeader(myMessage, "SendingMessage", "ValueSendingMessage");
            bus.SendLocal(myMessage);

            #endregion
        }

        public class MyMessage
        {

        }
    }
}