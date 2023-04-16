using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShippingCosts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingCosts.Tests
{
    [TestClass()]
    public class CalculationsTests
    {
        [TestMethod()]
        public void GetPriceTest()
        {
            List<Tuple<string, string, decimal>> prices = new()
            {              //Courier, Size, Price
                Tuple.Create("LP", "S", 1.5m),
                Tuple.Create("LP", "M", 4.9m),
                Tuple.Create("LP", "L", 6.9m),
                Tuple.Create("MR", "S", 2m),
                Tuple.Create("MR", "M", 3m),
                Tuple.Create("MR", "L", 4m)
            };

            decimal price1 = Calculations.GetPrice("LP", "M", prices);
            decimal price2 = Calculations.GetPrice("LP", "L", prices);
            decimal price3 = Calculations.GetPrice("MR", "L", prices);
            decimal price4 = Calculations.GetPrice("MR", "XL", prices);

            Assert.AreEqual(4.9m, price1);
            Assert.AreEqual(6.9m, price2);
            Assert.AreEqual(4m, price3);
            Assert.AreEqual(-1m, price4);
        }
        [TestMethod()]
        public void LowestPriceTest()
        {
            List<Tuple<string, string, decimal>> prices = new()
            {              //Courier, Size, Price
                Tuple.Create("LP", "S", 1.5m),
                Tuple.Create("LP", "M", 4.9m),
                Tuple.Create("LP", "L", 6.9m),
                Tuple.Create("MR", "S", 2m),
                Tuple.Create("MR", "M", 3m),
                Tuple.Create("MR", "L", 4m)
            };

            List<string> availableCouriers = prices.Select(d => d.Item1).ToList();

            decimal price1 = Calculations.GetLowestPrice("S", availableCouriers, prices);
            decimal price2 = Calculations.GetLowestPrice("M", availableCouriers, prices);
            decimal price3 = Calculations.GetLowestPrice("L", availableCouriers, prices);
            decimal price4 = Calculations.GetLowestPrice("XL", availableCouriers, prices);

            Assert.AreEqual(1.5m, price1);
            Assert.AreEqual(3m, price2);
            Assert.AreEqual(4m, price3);
        }
        [TestMethod()]
        public void CalculatePricesTest()
        {
            List<Tuple<string, string, decimal>> prices = new()
            {              //Courier, Size, Price
                Tuple.Create("LP", "S", 1.5m),
                Tuple.Create("LP", "M", 4.9m),
                Tuple.Create("LP", "L", 6.9m),
                Tuple.Create("MR", "S", 2m),
                Tuple.Create("MR", "M", 3m),
                Tuple.Create("MR", "L", 4m)
            };

            List<string> availableCouriers = prices.Select(d => d.Item1).ToList();
            List<string> availableSizes = prices.Select(d => d.Item2).ToList();

            List<Shipment> shipments = InputOutput.ReadData(@"input3.txt", availableSizes, availableCouriers);
            shipments = Calculations.SetPrices(shipments, "S", 10m, 3, "LP", "L", availableCouriers, prices);

            Assert.AreEqual(6.9m, shipments[0].Price);
            Assert.AreEqual(0m, shipments[0].Discount);

            Assert.AreEqual(6.9m, shipments[1].Price);
            Assert.AreEqual(0m, shipments[1].Discount);

            Assert.AreEqual(0m, shipments[2].Price);
            Assert.AreEqual(6.9m, shipments[2].Discount);

            Assert.AreEqual(1.5m, shipments[3].Price);
            Assert.AreEqual(0.5m, shipments[3].Discount);

            Assert.AreEqual(1.9m, shipments[9].Price);
            Assert.AreEqual(0.1m, shipments[9].Discount);

            Assert.AreEqual(2m, shipments[10].Price);
            Assert.AreEqual(0m, shipments[10].Discount);
        }
        [TestMethod()]
        public void SortMethodTest()
        {
            List<Tuple<string, string, decimal>> prices = new()
            {              //Courier, Size, Price
                Tuple.Create("LP", "S", 1.5m),
                Tuple.Create("LP", "M", 4.9m),
                Tuple.Create("LP", "L", 6.9m),
                Tuple.Create("MR", "S", 2m),
                Tuple.Create("MR", "M", 3m),
                Tuple.Create("MR", "L", 4m)
            };

            List<string> availableCouriers = prices.Select(d => d.Item1).ToList();
            List<string> availableSizes = prices.Select(d => d.Item2).ToList();

            List<Shipment> shipments = InputOutput.ReadData(@"input4.txt", availableSizes, availableCouriers);
            shipments.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

            Assert.AreEqual(1, shipments[0].Date.Day);
            Assert.AreEqual(2, shipments[1].Date.Day);
            Assert.AreEqual(6, shipments[5].Date.Day);
            Assert.AreEqual(8, shipments[6].Date.Day);
        }
    }
}