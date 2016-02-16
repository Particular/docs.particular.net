namespace Snippets4.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        public StaticHeaders()
        {
            Configure configure = Configure.With();

            #region header-static-endpoint

            using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
            {
                startableBus.OutgoingHeaders.Add("AllOutgoing", "ValueAllOutgoing");

                #endregion
            }
        }
    }
}