using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class IssuePaymentRequestHandler : IHandleMessages<IssuePaymentRequest>
{
	public IBus Bus { get; set; }

	public void Handle( IssuePaymentRequest message )
	{
		this.Bus.Publish<PaymentTransactionCompleted>( evt =>
		{
			evt.PaymentTransactionId = message.PaymentTransactionId;
		} );
	}
}
