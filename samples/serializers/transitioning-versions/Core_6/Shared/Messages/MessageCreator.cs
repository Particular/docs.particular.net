using System.Collections.Generic;

public static class MessageCreator
{
    public static CreateOrder NewOrder() =>
        new CreateOrder
        {
            OrderId = 9,
            OrderItems = new Dictionary<int, OrderItem>
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