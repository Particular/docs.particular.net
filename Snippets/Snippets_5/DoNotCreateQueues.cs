using NServiceBus;


public class DoNotCreateQueues
{
    public void Simple()
    {
        // start code DoNotCreateQueuesV5
        Configure.With(builder => builder.DoNotCreateQueues());
        // end code DoNotCreateQueuesV5
    }

}