namespace Core3.Headers
{
    using System.Collections.Generic;
    using NServiceBus;

    class StaticHeaders
    {
        public StaticHeaders()
        {
            Configure configure = Configure.With();

            #region header-static-endpoint

            using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
            {
                IDictionary<string, string> outgoingHeaders = ((IBus)startableBus).OutgoingHeaders;
                outgoingHeaders.Add("AllOutgoing", "ValueAllOutgoing");

                #endregion
            }
        }
    }
}