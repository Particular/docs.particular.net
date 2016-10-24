namespace Core5.Handlers
{
    using System;

    public class Order
    {
        public void AddLine(object product, object quantity)
        {
        }

        public bool HasLine(object lineId)
        {
            throw new NotImplementedException();
        }

        public bool HasProcessed(string id)
        {
            throw new NotImplementedException();
        }

        public void MarkAsProcessed(string id)
        {
        }
    }
}