using NServiceBus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaSample
{
  public class OrderSagaData : ContainSagaData
  {
    public string? OrderId { get; set; }
    public bool OrderProcessing { get; set; }

    public bool CustomerBilled { get; set; }
    public bool InventoryStaged { get; set; }
  }
}
