using Commands;
using NServiceBus.Logging;
using System.Diagnostics;

namespace Inventory;

public class UpdateProductStockHandler : IHandleMessages<UpdateProductStock>
{
    private static readonly ILog log = LogManager.GetLogger<UpdateProductStockHandler>();

    private static readonly ActivitySource source = new("Stock", "1.0.0");

    public Task Handle(UpdateProductStock message, IMessageHandlerContext context)
    {
        using Activity? activity = source.StartActivity("Stock_UpdateProductStock");
        var random = new Random(4);

        try 
        {
            var product = ProductStore.Products.Single(x => x.ProductId == message.ProductId);

            activity?.SetTag("ProductId", product.ProductId);
            activity?.AddEvent(new ActivityEvent("Stock_Recalculation_Starting"));

            // update stock
            // Introduces a transient exception to see how failed traces look like
            if (random.Next() % 2 == 0)
            {
                throw new Exception("Some transient exception");
            }

            activity?.AddEvent(new ActivityEvent("Stock_Recalculation_Completed"));
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            activity?.SetTag("otel.status_code", "ERROR");
            activity?.SetTag("otel.status_description", e.Message);
            throw;
        }
    }
}