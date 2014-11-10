using OnlineSales.Internal.Commands.Sales;

namespace OnlineSales.ECommerce.Controllers
{
    public interface ISubmitOrderSender
    {

        void Send(SubmitOrder message);

    }
}