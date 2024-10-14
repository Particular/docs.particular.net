using System;

namespace Billing;

public class OrderCalculator
{
    Random random = new Random();

    public decimal GetOrderTotal(string orderId)
    {
        // Retrieve order from database
        // Calculate price

        // Return the price
        return random.Next(25, 500);
    }
}