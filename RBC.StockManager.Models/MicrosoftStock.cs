using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC.StockManager.Models
{
    public class MicrosoftStock : StockBase
    {
        public MicrosoftStock(double price) : base("Microsoft", "MSFT", price)
        {

        }
    }
}
