//------------------------------------------------------------------------------
// This file is copied from the OnlineSales.ECommerce\Controllers
// It's copied because it's an auto-generated file and the #region's will get
// removed when the file is auto-generated.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NServiceBus;

using OnlineSales.Internal.Commands.Sales;


using OnlineSales.Sales;

#region OnlineSales.ECommerce.Controllers.TestMessagesController.auto
namespace OnlineSales.ECommerce.Controllers
{
    public partial class TestMessagesController : Controller
    {
        //
        // GET: /TestMessages/

        public ActionResult Index()
        {
            return View();
        }

				
		public ISubmitOrderSender SubmitOrderSender { get; set; }
		
        // POST: /TestMessages/SendMessageSubmitOrder
          
        [HttpPost]
        public string SendMessageSubmitOrder(SubmitOrder SubmitOrder)
        {
            ConfigureSubmitOrder(SubmitOrder);
            SubmitOrderSender.Send(SubmitOrder);
			return "<p> SubmitOrder command sent</p>";
        }
		

	  // Send Commands
    
        partial void ConfigureSubmitOrder(SubmitOrder message);
      
	 // Publish Events
    }
}
#endregion