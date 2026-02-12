namespace BuyersRemorseTimeoutClassDefinition;

#region BuyersRemorseTimeoutClassDefinition

class BuyersRemorsePolicy : Saga<BuyersRemorseData>,
    IAmStartedByMessages<PlaceOrder>
{
    // ...
}

class BuyersRemorseIsOver
{
}

#endregion

internal class Saga<T>
{
}

internal class BuyersRemorseData
{
}

internal class PlaceOrder
{
}

internal interface IAmStartedByMessages<T>
{
}