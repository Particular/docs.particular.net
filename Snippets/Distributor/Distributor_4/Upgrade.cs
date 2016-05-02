using NServiceBus;

class Upgrade
{

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
}