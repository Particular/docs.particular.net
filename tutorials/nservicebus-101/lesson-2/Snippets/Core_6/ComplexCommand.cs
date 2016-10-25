using System.Collections.Generic;
using NServiceBus;

namespace Core_6
{
    #region ComplexCommand

    public class DoSomethingComplex : ICommand
    {
        public int SomeId { get; set; }
        public ChildClass ChildStuff { get; set; }
        public List<ChildClass> ListOfStuff { get; set; }

        public DoSomethingComplex()
        {
            ListOfStuff = new List<ChildClass>();
        }
    }

    public class ChildClass
    {
        public string SomeProperty { get; set; }
    }

    #endregion
}