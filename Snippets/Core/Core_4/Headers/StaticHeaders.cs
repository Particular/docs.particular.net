namespace Core4.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        StaticHeaders(Configure configure)
        {
            #region header-static-endpoint

            using (var startableBus = configure.UnicastBus().CreateBus())
            {
                startableBus.OutgoingHeaders.Add("AllOutgoing", "ValueAllOutgoing");

                #endregion
            }
        }
    }
}