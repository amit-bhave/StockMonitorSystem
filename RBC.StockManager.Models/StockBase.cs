using RBC.StockManager.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC.StockManager.Models
{
    public abstract class StockBase
    {
        private string _name;
        private string _symbol;
        private double _price;

        private List<IInvestor> _investors = new List<IInvestor>();

        public string Name { get { return _name; } set { _name = value; } }
        public string Symbol { get { return _symbol; } set { _symbol = value; } }

        public double Price { get { return _price; } }
        public List<IInvestor> Investors { get { return _investors; } }


        public StockBase(string name, string symbol, double price)
        {
            _name = name;
            _symbol = symbol;
            _price = price;
        }

        public void Attach(IInvestor investor)
        {
            _investors.Add(investor);
        }

        public void Detach(IInvestor investor)
        {            
            _investors.Remove(investor);
        }

        protected void Notify(double oldprice, double newprice)
        {
            foreach (var investor in _investors)
            {
                if (!investor.StockTriggers.ContainsKey(this._symbol)) return;

                foreach (var trigger in investor.StockTriggers[this._symbol])
                {
                        if (!trigger.ClientNotified)
                        {
                            if((newprice > oldprice && trigger.Direction == Enums.TriggerDirection.FromBelow && newprice >= trigger.Threshold) ||
                                (newprice < oldprice && trigger.Direction == Enums.TriggerDirection.FromAbove && newprice <= trigger.Threshold))
                            {
                                investor.ActOnNotification($"{investor.Name} {trigger.Type.ToString()} Stock {this._name}({this._symbol}) at price of {newprice}");
                                trigger.ClientNotified = true;
                            }

                            break;
                        }

                        var delta = Math.Abs(newprice - oldprice);

                        if(delta > trigger.Sensitivity)
                        {
                            if ((newprice > oldprice && trigger.Direction == Enums.TriggerDirection.FromBelow && newprice >= trigger.Threshold && oldprice < trigger.Threshold) ||
                                (newprice < oldprice && trigger.Direction == Enums.TriggerDirection.FromAbove && newprice <= trigger.Threshold && oldprice > trigger.Threshold))
                            {
                                investor.ActOnNotification($"{investor.Name} {trigger.Type.ToString()} Stock {this._name}({this._symbol}) at price of {newprice}");
                            }
                        }
                    
                }
            }
        }

        public void SetPrice(double price)
        {
            if (price < 0.00)
                throw new InvalidOperationException("Stock Price Can't be negative.");
            if (price == this._price)
                return;
            var oldprice = this._price;
            this._price = price;
            Notify(oldprice, price);
        }
        
    }
}
