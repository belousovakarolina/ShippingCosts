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
    public class ShipmentTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            Shipment shipment1 = new Shipment(new DateTime(2015, 02, 14), "S", "MR");
            shipment1.Price = 4m;
            shipment1.Discount = 0m;
            Shipment shipment2 = new Shipment("2023-02-14 CUSPS");
            Assert.AreEqual("2015-02-14 S MR 4,00 -", shipment1.ToString());
            Assert.AreEqual("2023-02-14 CUSPS Ignored", shipment2.ToString());

        }
    }
}