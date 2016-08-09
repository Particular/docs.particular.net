using System;
using System.Collections.Generic;

public static class MesasgeBuilder
{

    public static CreateOrder BuildMessage()
    {
        return new CreateOrder
        {
            OrderId = 9,
            Date = DateTime.Now,
            CustomerId = 12,
            OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    ItemId = 6,
                    Quantity = 2
                },
                new OrderItem
                {
                    ItemId = 5,
                    Quantity = 4
                },
            }
        };
    }
}