namespace Wcf_3.Enum
{
    using NServiceBus;

    class Usage
    {
        #region WcfEnumCallback

        class MyService :
            WcfService<Message, Status>
        {
        }

        #endregion
    }
}