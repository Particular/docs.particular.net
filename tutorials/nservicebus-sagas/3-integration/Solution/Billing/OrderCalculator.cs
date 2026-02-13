namespace Billing;

public class OrderCalculator
{
    public decimal GetOrderTotal(string orderId)
    {
        // Retrieve order from database
        // Calculate price

        // Return the price
        return Random.Shared.Next(25, 500);
    }
}