namespace Core3.UpgradeGuides._3to4
{
    using NServiceBus;

    class Upgrade
    {

        void RenamePrincipalHack(Configure configure)
        {
            #region 3to4RenamePrincipalHack

            var unicastBus = configure.UnicastBus();
            unicastBus.ImpersonateSender(true);

            #endregion
        }

        void DisableSecondLevelRetries(Configure configure)
        {
            #region 3to4DisableSecondLevelRetries

            configure.DisableSecondLevelRetries();

            #endregion
        }

        void EnableSagas(Configure configure)
        {
            #region 3to4EnableSagas

            configure.Sagas();

            #endregion
        }

        void SetMessageHeader(IBus bus)
        {
            #region 3to4SetMessageHeader

            var myMessage = new MyMessage();
            myMessage.SetHeader("SendingMessage", "ValueSendingMessage");
            bus.SendLocal(myMessage);

            #endregion
        }

        public class MyMessage
        {

        }

    }
}