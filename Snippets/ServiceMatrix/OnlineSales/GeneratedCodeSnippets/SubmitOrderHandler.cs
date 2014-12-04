//------------------------------------------------------------------------------
// This file is copied from the OnlineSales.OrderProcessing\Infrastructure
// It's copied because it's an auto-generated file and the #region's will get
// removed when the file is auto-generated.
//------------------------------------------------------------------------------

using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Sales;


#region ServiceMatrix.OnlineSales.Sales.SubmitOrderHandler.auto
namespace OnlineSales.Sales
{
    public partial class SubmitOrderHandler : IHandleMessages<SubmitOrder>
    {
		public void Handle(SubmitOrder message)
		{
			// Handle message on partial class
			this.HandleImplementation(message);
		}

		partial void HandleImplementation(SubmitOrder message);
        public IBus Bus { get; set; }
    }
}
#endregion
