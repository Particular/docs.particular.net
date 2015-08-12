using NServiceBus.Saga;

namespace Snippets6.Sagas.FindSagas
{
    public class MySagaData : ContainSagaData
    {
        public string SomeID { get; set; }
        public string SomeData { get; set; }
    }
}