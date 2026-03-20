using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public record PlaceOrder(Guid OrderId, string Product, int Quantity) : ICommand
    {
    }
}
