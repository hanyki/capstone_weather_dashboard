﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for AddressTest
    /// </summary>
    [TestClass]
    public class AddressTest
    {
        [TestMethod]
        public void TestAddressSearch()
        {
            var addresses = Address.Search("Lansing MI");
            Assert.AreEqual(1, addresses.Count());
            var address = addresses.Single();
            Assert.IsNull(address.StreetAddress);
            Assert.AreEqual("Lansing", address.City);
            Assert.AreEqual("Ingham", address.County);
            Assert.AreEqual(new State("MI"), address.State);

            double accuracy = 0.25;
            Assert.IsTrue(Math.Abs(42.7980673 - address.Latitude) < accuracy);
            Assert.IsTrue(Math.Abs(-84.4274753 - address.Longitude) < accuracy);
        }
    }
}
