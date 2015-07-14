namespace Snippets4
{
    using NServiceBus;

    public class DoNotCreateQueues
    {
        public void Simple()
        {
            #region DoNotCreateQueues
            Configure.With().DoNotCreateQueues();
            #endregion
        }

    }
}