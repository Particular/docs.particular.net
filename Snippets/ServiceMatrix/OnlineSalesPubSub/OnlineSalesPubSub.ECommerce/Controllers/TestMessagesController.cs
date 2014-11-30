using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSalesPubSub.Contracts.Marketing;
using OnlineSalesPubSub.Marketing;

namespace OnlineSalesPubSub.ECommerce.Controllers
{
    public partial class TestMessagesController
    {
        // TODO: OnlineSalesPubSub.ECommerce: Configure sent/published messages' properties implementing the partial Configure[MessageName] method.");
    }

    public class TestMessagesControllerSample : Controller
    {
        #region ServiceMatrix.OnlineSalesPubSub.ECommerce.TestMessagesController
        // POST: /TestMessages/SendMessageBrowsedProduct

        [HttpPost]
        public string SendMessageBrowsedProduct(BrowsedProduct BrowsedProduct)
        {
            ConfigureBrowsedProduct(BrowsedProduct);
            MvcApplication.Bus.Publish(BrowsedProduct);
            return "<p> BrowsedProduct published</p>";
        }
        #endregion

        private void ConfigureBrowsedProduct(object o)
        {

        }
    }
}