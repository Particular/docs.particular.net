using System;
using NServiceBus;
using OnlineSalesPubSub.Contracts.Marketing;


namespace OnlineSalesPubSub.Marketing
{
    #region ServiceMatrix.OnlineSalesPubSub.Marketing.BrowsedProductHandler
    public partial class BrowsedProductHandler
    {
		
        partial void HandleImplementation(BrowsedProduct message)
        {
            // TODO: BrowsedProductHandler: Add code to handle the BrowsedProduct message.
            Console.WriteLine("Marketing received " + message.GetType().Name);
        }

    }
    #endregion
}
