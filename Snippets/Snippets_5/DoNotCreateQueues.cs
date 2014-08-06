using NServiceBus;


public class DoNotCreateQueues
{
    public void Simple()
    {
        #region DoNotCreateQueuesV5

        Configure.With(builder => builder.DoNotCreateQueues());

        #endregion
    }

}