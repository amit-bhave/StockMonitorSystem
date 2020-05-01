using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC.StockManager.Models.Interfaces
{
    public interface IInvestor
    {
        int InvestorId { get; set; }
        string Name { get; set; }

        Dictionary<string, List<Trigger>> StockTriggers { get; set; }

        void ActOnNotification(string message);
    }
}
