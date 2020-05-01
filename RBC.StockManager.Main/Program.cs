using RBC.StockManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC.StockManager.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var msstock = new MicrosoftStock(10.00);

            var msinv1triggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Buy,
                    Threshold = 8.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 0.5,
                    ClientNotified = false
                },
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromBelow,
                    Sensitivity = 0.5,
                    ClientNotified = false
                }
            };

            var msinv2triggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Buy,
                    Threshold = 9.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 1.0,
                    ClientNotified = false
                },
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 12.00,
                    Direction = Models.Enums.TriggerDirection.FromBelow,
                    Sensitivity = 1.0,
                    ClientNotified = false
                }
            };

            var inv1triggers = new Dictionary<string, List<Trigger>>();
            inv1triggers.Add("MSFT", msinv1triggers);
            var inv1 = new Investor(1, "Investor 1", inv1triggers);

            var inv2triggers = new Dictionary<string, List<Trigger>>();
            inv2triggers.Add("MSFT", msinv2triggers);
            var inv2 = new Investor(2, "Investor 2", inv2triggers);

            msstock.Attach(inv1);
            msstock.Attach(inv2);

            msstock.SetPrice(9.00); // Investor 2 Notified. First Notification. Sensitivity Ignored.
            msstock.SetPrice(8.25); // Investor 2 not notified as Old Price was already at or below threshold
            msstock.SetPrice(8.75); // No Notification Upward Trend.
            msstock.SetPrice(8.25); // Investor 2 not notified as Old Price was already at or below threshold
            msstock.SetPrice(8.00); // Investor 1 Notified. First Notification.Sensitivity Ignored.
            msstock.SetPrice(9.50); // No Notification Upward Trend.
            msstock.SetPrice(8.75); // No Notification for Investor 2 as drop within sensitivity range.
            msstock.SetPrice(9.50); // No Notification Upward Trend.
            msstock.SetPrice(8.49); // Investor 2 notified as drop outside sensitivity limits.
            msstock.SetPrice(9.99); // No Notification Upward Trend.
            msstock.SetPrice(9.00); // Investor 2 is not notified as drop within sensitivity range.
            msstock.SetPrice(7.99); // Investor 1 Notified. Investor 2 not notified as Old Price was already at or below threshold
            msstock.SetPrice(6.98); // No Notification as old price is already at threshold or below for both investors.
            Console.ReadKey();
        }
    }
}
