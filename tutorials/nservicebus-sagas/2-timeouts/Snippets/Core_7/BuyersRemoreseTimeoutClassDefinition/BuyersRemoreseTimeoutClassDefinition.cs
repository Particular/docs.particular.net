namespace Core_7.BuersRemoreseTimeoutClassDefinition
{
    #region BuyersRemoreseTimeoutClassDefinition

    class BuyersRemorsePolicy : Saga<BuyersRemorseState>,
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

    internal class BuyersRemorseState
    {
    }

    internal class PlaceOrder
    {
    }

    internal interface IAmStartedByMessages<T>
    {
    }
}
