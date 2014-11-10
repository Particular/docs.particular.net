using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Sales;

#region ServiceMatrix.OnlineSales.Sales.SubmitOrderSender
namespace OnlineSales.Sales
{
    public partial class SubmitOrderSender
    {
        //You can add the partial method to change the submit order message.
        partial void ConfigureSubmitOrder(SubmitOrder message)
        {
            message.CustomerName = "John Doe";
        }
    }
}
#endregion
