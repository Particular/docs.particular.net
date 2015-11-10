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
        
        public void RunDistributor()
        {
            #region 3to4RunDistributor

            Configure configure = Configure.With();
            configure.RunDistributor();

            #endregion
        }
        public void EnlistWithDistributor()
        {
            #region 3to4EnlistWithDistributor

            Configure configure = Configure.With();
            configure.EnlistWithDistributor();

            #endregion
        }
        
        public void SetMessageHeader()
        {
            IBus bus = null;

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