using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Sales;

namespace OnlineSales.Sales
{
    #region ServiceMatrix.OnlineSales.Sales.SubmitOrderHandler
    public partial class SubmitOrderHandler
    {
        partial void HandleImplementation(SubmitOrder message)
        {
            // TODO: SubmitOrderHandler: Add code to handle the SubmitOrder message.
            Console.WriteLine("Sales received " + message.GetType().Name);
        
            var orderAccepted = new OnlineSales.Contracts.Sales.OrderAccepted();

            //Access inbound messages and set outbound message properties
            //orderAccepted.ReferenceNumber = message.OrderID;
            
            Bus.Publish(orderAccepted);
        }
    }
    #endregion
}

namespace OnlineSales.Sales.Example2
{
    #region ServiceMatrix.OnlineSales.Sales.SubmitOrderHandler.before
    public partial class SubmitOrderHandler
    {
        partial void HandleImplementation(SubmitOrder message)
        {
            // TODO: SubmitOrderHandler: Add code to handle the SubmitOrder message.
            Console.WriteLine("Sales received " + message.GetType().Name);
        }
    }
    #endregion

    public partial class SubmitOrderHandler
    {
        partial void HandleImplementation(SubmitOrder message);
    }
}

namespace OnlineSales.Sales.Example
{
    #region ServiceMatrix.OnlineSales.Sales.SubmitOrderHandler.exception
    public partial class SubmitOrderHandler
    {
        partial void HandleImplementation(SubmitOrder message)
        {
            Console.WriteLine("Sales received " + message.GetType().Name);

            //Throw an exception to simulate an error!
            throw new Exception("Oh no.. something bad happened");
        }
    }
    #endregion

    public partial class SubmitOrderHandler
    {
        partial void HandleImplementation(SubmitOrder message);
    }
}
