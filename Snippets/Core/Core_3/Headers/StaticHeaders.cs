namespace Core3.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        public StaticHeaders()
        {
            var configure = Configure.With();

            #region header-static-endpoint

            using (var startableBus = configure.UnicastBus().CreateBus())
            {
                var outgoingHeaders = ((IBus)startableBus).OutgoingHeaders;
                outgoingHeaders.Add("AllOutgoing", "ValueAllOutgoing");

                #endregion
            }
        }
    }
}