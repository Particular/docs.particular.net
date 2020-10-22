using System;

namespace Billing
{
    public class OrderCalculator
    {
        Random rnd = new Random();

        public decimal GetOrderTotal(string orderId)
        {
            // Retrieve order from database
            // Calculate price

            // Return the price
            return rnd.Next(25, 500);
        }
    }
}