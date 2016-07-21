namespace Core4.UpgradeGuides._3to4
{
    using System.Transactions;
    using NServiceBus;

    class Upgrade
    {

        void RenamePrincipalHack(Configure configure)
        {
            #region 3to4RenamePrincipalHack

            var unicastBus = configure.UnicastBus();
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

        void SetMessageHeader(IBus bus)
        {
            #region 3to4SetMessageHeader

            var myMessage = new MyMessage();
            bus.SetMessageHeader(
                msg: myMessage,
                key: "SendingMessage",
                value: "ValueSendingMessage");
            bus.SendLocal(myMessage);

            #endregion
        }

        public class MyMessage
        {

        }
    }
}