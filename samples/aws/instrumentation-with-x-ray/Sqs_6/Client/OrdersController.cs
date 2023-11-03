using System.Diagnostics;
using Commands;
using Domain;
using Events;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;

namespace RootCauseExample.Client;

public class OrdersController : Controller
{
    private readonly IMessageSession messageSession;

    public OrdersController(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    [Route("orders/create/")]
    [HttpGet]
    public async Task<IActionResult> Create(string customerId)
    {
        var orderId = Guid.NewGuid();
        
        Console.WriteLine($"TraceID in Controller: {Activity.Current?.TraceId}");

        await messageSession.Send(new PlaceOrder
        {
            CustomerId = new Guid("15600CE9-2437-4804-88AD-93E673BFA8EB"),
            Order = new OrderDetails
            {
                OrderId = orderId,
                Lines = new List<OrderLine>{ new()
                {
                    Product = new Product
                    {
                        ProductId = "DD3FC711-4693-44C5-B629-779F393C9C3A",
                        ProductName = "Fantastic beasts and where to find them"
                    }
                } }
            }
        }).ConfigureAwait(false);

        return Ok();
    }
}