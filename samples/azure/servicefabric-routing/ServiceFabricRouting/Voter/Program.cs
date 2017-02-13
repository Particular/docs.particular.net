using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Voter
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("Voter");
        }
    }
}
