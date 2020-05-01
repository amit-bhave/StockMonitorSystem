using RBC.StockManager.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC.StockManager.Models
{
    public class Investor : IInvestor
    {
        public int InvestorId { get; set; }
        public string Name { get; set; }

        public Dictionary<string, List<Trigger>> StockTriggers { get; set; }

        public Investor(int investorid, string name)
        {
            InvestorId = investorid;
            Name = name;            
        }

        public Investor(int investorid, string name, Dictionary<string, List<Trigger>> triggers)
        {
            InvestorId = investorid;
            Name = name;
            StockTriggers = triggers;
        }

        public void ActOnNotification(string message)
        {
            Console.WriteLine(message);
        }
    }
}
