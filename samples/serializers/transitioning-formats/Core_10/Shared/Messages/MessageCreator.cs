using System.Collections.Generic;

public static class MessageCreator
{
    public static Order NewOrder() =>
        new Order
        {
            OrderId = 9,
            OrderItems =
                new Dictionary<int, OrderItem>
                {
                    {
                        3,
                        new OrderItem
                        {
                            Quantity = 2
                        }
                    },
                    {
                        8,
                        new OrderItem
                        {
                            Quantity = 7
                        }
                    },
                }
        };
}