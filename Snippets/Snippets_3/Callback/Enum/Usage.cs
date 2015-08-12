namespace Snippets3.Callback.Enum
{
    using System;
    using NServiceBus;

    class Usage
    {

        void Simple()
        {
            IBus bus = null;
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