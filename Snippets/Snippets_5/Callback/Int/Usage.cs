namespace Snippets5.Callback.Int
{
    using System;
    using NServiceBus;

    class Usage
    {

        void Simple()
        {
            IBus bus = null;
            #region IntCallback

            Message message = new Message();
            bus.Send(message)
                .Register<int>(response =>
                {
                    Console.WriteLine("Callback received with response:" + response);
                });

            #endregion
        }

    }
}