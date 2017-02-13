using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Messages
{
    public class PlaceVote : ICommand
    {
        public string ZipCode { get; set; }

        public string Candidate { get; set; }
    }
}
