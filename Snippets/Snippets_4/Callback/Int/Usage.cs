using System;
using NServiceBus;

namespace Snippets4.Callback.Int
{

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

            Console.WriteLine("Message sent");
        }

    }
}