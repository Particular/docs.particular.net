namespace Core6.Enum
{
    using NServiceBus;

    class Usage
    {
        #region WcfEnum
        class MyService : WcfService<Message, Status>
        {
        }
        #endregion
    }
}
