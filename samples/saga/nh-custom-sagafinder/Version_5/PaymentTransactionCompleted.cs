using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface PaymentTransactionCompleted : IEvent
{
	String PaymentTransactionId { get; set; }
}
