using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC.StockManager.Models
{
    public class GoogleStock : StockBase
    {
        public GoogleStock(double price) : base("Google", "GOGL", price)
        {

        }
    }
}
