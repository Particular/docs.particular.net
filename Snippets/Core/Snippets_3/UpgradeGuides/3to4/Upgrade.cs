namespace Snippets3.UpgradeGuides._3to4
{
    using NServiceBus;
    using NServiceBus.Unicast.Config;

    class Upgrade
    {

        void RenamePrincipalHack(Configure configure)
        {
            #region 3to4RenamePrincipalHack

            ConfigUnicastBus unicastBus = configure.UnicastBus();
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

        void RunDistributor(Configure configure)
        {
            #region 3to4RunDistributor

            configure.RunDistributor();

            #endregion
        }

        void EnlistWithDistributor(Configure configure)
        {
            #region 3to4EnlistWithDistributor

            configure.EnlistWithDistributor();

            #endregion
        }

        void SetMessageHeader(IBus bus)
        {
            #region 3to4SetMessageHeader

            MyMessage myMessage = new MyMessage();
            myMessage.SetHeader("SendingMessage", "ValueSendingMessage");
            bus.SendLocal(myMessage);

            #endregion
        }

        public class MyMessage
        {

        }

    }
}