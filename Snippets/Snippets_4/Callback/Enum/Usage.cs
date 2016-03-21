namespace Snippets4.Callback.Enum
{
    using System;
    using NServiceBus;

    class Usage
    {

        Usage(IBus bus)
        {
            #region EnumCallback

            Message message = new Message();
            bus.Send(message)
                .Register<Status>(response =>
                {
                    Console.WriteLine("Callback received with response:" + response);
                });

            #endregion
        }
    }
}