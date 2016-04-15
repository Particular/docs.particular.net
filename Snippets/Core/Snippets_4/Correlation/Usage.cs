namespace Core4.Correlation
{
    using NServiceBus;

    class Usage
    {
        Usage(IBus bus)
        {
            #region custom-correlationid

            bus.Send("TargetQueue","My custom correlation id", new MyRequest());

            #endregion
        }

    }
}