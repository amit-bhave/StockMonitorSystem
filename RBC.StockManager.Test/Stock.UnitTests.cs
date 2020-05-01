using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RBC.StockManager.Models;
using RBC.StockManager.Models.Interfaces;
using System.Collections.Generic;

namespace RBC.StockManager.Test
{
    class StockUnitTestCaseExtension : StockBase
    {
        public StockUnitTestCaseExtension(string name, string symbol, double price)
                : base(name, symbol, price)
        {

        }

        public void NotifyWrapper(double oldprice, double newprice)
        {
            base.Notify(oldprice, newprice);
        }
    }

    class UnitTestInvestor: IInvestor
    {
        public int NotificationCount { get; set; }
        public int InvestorId { get; set; }
        public string Name { get; set; }

        public Dictionary<string, List<Trigger>> StockTriggers { get; set; }

        public void ActOnNotification(string message)
        {
            this.NotificationCount++;
        }
    }

    [TestClass]
    public class StockUnitTest
    {
        #region Price Change Test Cases

        [TestMethod]
        public void VerifyPriceChangeIsSet()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            //Act
            utstock.SetPrice(15.00);

            //Assert
            Assert.AreEqual(15.00, utstock.Price);

        }

        [TestMethod]        
        public void VerifyNegativeStockPriceNotAllowed()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            // Act => Assert Exception
            Assert.ThrowsException<InvalidOperationException>(() => utstock.SetPrice(-1.00));
        }

        #endregion

        #region Subscription Test Cases

        [TestMethod]
        public void VerifyInvestorsAddedToSubscriberList()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);
            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor"
            };

            //Act
            utstock.Attach(utinvestor);

            //Assert
            Assert.AreEqual(1, utstock.Investors.Count);
        }

        [TestMethod]
        public void VerifyInvestorsRemovedFromSubscriberList()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);
            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor"
            };

            utstock.Attach(utinvestor);

            //Act
            utstock.Detach(utinvestor);
            
            //Assert
            Assert.AreEqual(0, utstock.Investors.Count);
        }

        #endregion

        #region First Time Notification Test Cases

        [TestMethod]
        public void VerifyInvestorNotifiedFirstTimePriceReachedThresholdFromAbove()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Buy,
                    Threshold = 8.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 0.5,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(8.00);

            //Assert
            Assert.AreEqual(1, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotifiedFirstTimePriceCrossedThresholdFromAbove()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Buy,
                    Threshold = 8.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 0.5,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(7.75);

            //Assert
            Assert.AreEqual(1, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotifiedFirstTimePriceReachedThresholdFromBelow()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromBelow,
                    Sensitivity = 0.5,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(15.00);

            //Assert
            Assert.AreEqual(1, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotifiedFirstTimePriceCrossedThresholdFromBelow()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromBelow,
                    Sensitivity = 0.5,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(15.25);

            //Assert
            Assert.AreEqual(1, utinvestor.NotificationCount);
        }

        #endregion

        #region No Notification Test Cases

        [TestMethod]
        public void VerifyInvestorNotNotifiedFirstTimeThresholdReachedFromOppositeDirectionOfTrigger()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 0.5,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(15.00);

            //Assert
            Assert.AreEqual(0, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotNotifiedFirstTimeThresholdCrossedFromOppositeDirectionOfTrigger()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 0.5,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(15.50);

            //Assert
            Assert.AreEqual(0, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotNotifiedWhenThresholdIsNotReachedOrCrossed()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromBelow,
                    Sensitivity = 1.00,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(11.25);
            utstock.SetPrice(11.50);
            utstock.SetPrice(14.50);
            utstock.SetPrice(14.75);
            utstock.SetPrice(14.99);

            //Assert
            Assert.AreEqual(0, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotNotifiedWhenOldPriceIsAlreadyAboveThreshold()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromBelow,
                    Sensitivity = 1.00,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(15.00);
            utstock.SetPrice(14.50);
            utstock.SetPrice(15.25);
            utstock.SetPrice(15.75);
            utstock.SetPrice(17.75);

            //Assert
            Assert.AreEqual(1, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotNotifiedWhenOldPriceIsAlreadyBelowThreshold()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 10.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Buy,
                    Threshold = 8.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 1.00,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(8.00);
            utstock.SetPrice(8.50);
            utstock.SetPrice(7.75);
            utstock.SetPrice(7.25);
            utstock.SetPrice(5.25);

            //Assert
            Assert.AreEqual(1, utinvestor.NotificationCount);
        }

        #endregion

        #region Trigger Sensitivity Test Cases

        [TestMethod]
        public void VerifyInvestorNotifiedWhenThresholdReachedOutsideFromDelta()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 20.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 1.00,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(15.00);
            utstock.SetPrice(16.01);
            utstock.SetPrice(15.00);
            utstock.SetPrice(13.50);
            utstock.SetPrice(15.00);

            //Assert
            Assert.AreEqual(2, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotifiedWhenThresholdCrossedOutsideFromDelta()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 20.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 1.00,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(15.00);
            utstock.SetPrice(16.01);
            utstock.SetPrice(14.99);
            utstock.SetPrice(13.50);
            utstock.SetPrice(15.00);

            //Assert
            Assert.AreEqual(2, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotNotifiedWhenThresholdReachedWithinFromDelta()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 20.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 1.00,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(15.00);
            utstock.SetPrice(15.99);
            utstock.SetPrice(15.00);
            utstock.SetPrice(13.50);
            utstock.SetPrice(15.00);

            //Assert
            Assert.AreEqual(1, utinvestor.NotificationCount);
        }

        [TestMethod]
        public void VerifyInvestorNotNotifiedWhenThresholdCrossedWithinFromDelta()
        {
            // Arrange
            var utstock = new StockUnitTestCaseExtension("TestStock", "TEST", 20.00);

            var uttriggers = new List<Trigger>()
            {
                new Trigger()
                {
                    Type = Models.Enums.TriggerType.Sell,
                    Threshold = 15.00,
                    Direction = Models.Enums.TriggerDirection.FromAbove,
                    Sensitivity = 1.00,
                    ClientNotified = false
                }
            };

            var utinvestor = new UnitTestInvestor()
            {
                InvestorId = 1,
                Name = "TestInvestor",
                StockTriggers = new Dictionary<string, List<Trigger>>()

            };

            utinvestor.StockTriggers.Add("TEST", uttriggers);

            utstock.Attach(utinvestor);

            //Act
            utstock.SetPrice(15.00);
            utstock.SetPrice(15.99);
            utstock.SetPrice(14.99);
            utstock.SetPrice(13.50);
            utstock.SetPrice(15.00);

            //Assert
            Assert.AreEqual(1, utinvestor.NotificationCount);
        }

        #endregion

    }
}
