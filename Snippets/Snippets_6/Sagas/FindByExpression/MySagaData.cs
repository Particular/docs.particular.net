using NServiceBus.Saga;

namespace Snippets6.Sagas.FindByExpression
{
    public class MySagaData : ContainSagaData
    {
        public string SomeID { get; set; }
    }
}