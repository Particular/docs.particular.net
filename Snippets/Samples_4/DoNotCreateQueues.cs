using NServiceBus;


public class DoNotCreateQueues
{
    public void Simple()
    {
        // start code DoNotCreateQueuesV4
        Configure.With().DoNotCreateQueues();
        // end code DoNotCreateQueuesV4
    }

}