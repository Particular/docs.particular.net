using NServiceBus;

class Upgrade
{

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
}