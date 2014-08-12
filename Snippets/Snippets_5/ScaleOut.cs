using NServiceBus;


public class ScaleOut
{
    public void Simple()
    {
        #region ScaleOutV5

        Configure.With(b => b.ScaleOut(settings =>
        {
            settings.UseSingleBrokerQueue();
            //or
            settings.UseUniqueBrokerQueuePerMachine();
        }));

        #endregion
    }

}