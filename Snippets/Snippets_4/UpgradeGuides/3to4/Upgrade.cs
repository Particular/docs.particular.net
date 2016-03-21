namespace Snippets4.UpgradeGuides._3to4
{
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Unicast.Config;

    class Upgrade
    {

        void RenamePrincipalHack(Configure configure)
        {
            #region 3to4RenamePrincipalHack
            ConfigUnicastBus unicastBus = configure.UnicastBus();
            unicastBus.RunHandlersUnderIncomingPrincipal(true);
            #endregion
        }

        void RevertToSerializable()
        {
            #region 3to4RevertToSerializable

            Configure.Transactions.Advanced(settings =>
                settings.IsolationLevel(IsolationLevel.Serializable));

            #endregion
        }

        void DisableSecondLevelRetries()
        {
            #region 3to4DisableSecondLevelRetries

            Configure.Features.Disable<NServiceBus.Features.SecondLevelRetries>();

            #endregion
        }

        void EnableSagas()
        {
            #region 3to4EnableSagas

            Configure.Features.Enable<NServiceBus.Features.Sagas>();

            #endregion
        }

        void RunDistributor(Configure configure)
        {
            #region 3to4RunDistributor

            configure.RunMSMQDistributor();

            #endregion
        }
        void EnlistWithDistributor(Configure configure)
        {
            #region 3to4EnlistWithDistributor

            configure.EnlistWithMSMQDistributor();

            #endregion
        }

        void SetMessageHeader(IBus bus)
        {
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