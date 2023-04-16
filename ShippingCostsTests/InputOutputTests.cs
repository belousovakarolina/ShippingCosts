using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShippingCosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingCosts.Tests
{
    [TestClass()]
    public class InputOutputTests
    {
        [TestMethod()]
        public void ReadDataTest()
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

            List<Shipment> shipments = InputOutput.ReadData(@"input2.txt", availableSizes, availableCouriers);

            Assert.AreEqual(5, shipments.Count);
            Assert.AreEqual(false, shipments[0].Ignored);
            Assert.AreEqual("MR", shipments[1].Courier);
            Assert.AreEqual(true, shipments[2].Ignored);
            Assert.AreEqual("2015-02-29 CUSPS", shipments[3].Data);
            Assert.AreEqual("L", shipments[4].Size);
        }
    }
}