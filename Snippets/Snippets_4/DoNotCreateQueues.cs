using NServiceBus;


public class DoNotCreateQueues
{
    public void Simple()
    {
        #region DoNotCreateQueuesV4
        Configure.With().DoNotCreateQueues();
        #endregion
    }

}