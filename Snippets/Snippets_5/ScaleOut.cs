using NServiceBus;


public class ScaleOut
{
    public void Simple()
    {
        #region ScaleOutV5

        Configure.With(b =>
        {
            b.ScaleOut().UseSingleBrokerQueue();
            //or
            b.ScaleOut().UseUniqueBrokerQueuePerMachine();
        });

        #endregion
    }

}