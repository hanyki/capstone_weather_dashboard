﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherStation.WeatherEventProviders;

namespace WeatherStation.Tests
{
    /// <summary>
    /// Summary description for NcdcTest
    /// </summary>
    [TestClass]
    public class NcdcTest
    {
        private readonly Ncdc _ncdc = new Ncdc();

        [TestMethod]
        public void TestSingleDaySearch()
        {
            var date = new DateTime(2008, 6, 6);
            var events = _ncdc.GetEvents(new State("MI"), "Ingham", date, date, null);
            Assert.AreEqual(1, events.Count());
            var e = events.Single();
            Assert.AreEqual(date, e.Date);
            Assert.AreEqual(WeatherIncidentType.HighWind, e.EventType);
            var location = e.Locations.Single();
            Assert.IsNull(location.StreetAddress);
            Assert.AreEqual("North Leslie", location.City);
            Assert.AreEqual("Ingham", location.County);
            Assert.AreEqual(new State("MI"), location.State);
            Assert.AreEqual(42.4888206, location.Latitude);
            Assert.AreEqual(-84.4281308, location.Longitude);
        }

        [TestMethod]
        public void TestZoneLocationReturned()
        {
            var date = new DateTime(2008, 12, 19);
            var events = _ncdc.GetEvents(new State("MI"), "Ingham", date, date, null);
            var e = events.Single();
            var miz051Zips = new[]
                                 {
                                     "48811", "48812", "48818", "48829", "48834", "48838", "48850", "48852", "48884",
                                     "48885", "48886", "48888", "48891", "49322", "49326", "49329", "49339", "49347"
                                 };
            var miz059Zips = new[]
                                 {
                                     "48808", "48820", "48822", "48831", "48833", "48835", "48853", "48866", "48879",
                                     "48894"
                                 };
            var miz067Zips = new[]
                                 {
                                     "48805", "48819", "48823", "48824", "48825", "48826", "48840", "48842", "48854",
                                     "48864", "48892", "48895", "48901", "48906", "48909", "48910", "48911", "48912",
                                     "48913", "48915", "48916", "48918", "48919", "48921", "48922", "48924", "48929",
                                     "48930", "48933", "48937", "48950", "48956", "48980", "49251", "49264", "49285"
                                 };

            var allZips = miz051Zips.Concat(miz059Zips).Concat(miz067Zips);

            foreach(var location in e.Locations)
            {
                Assert.IsTrue(allZips.Contains(location.ZipCode));
            }
        }

        [TestMethod]
        public void TestMultipleDaySearch()
        {
            var date = new DateTime(2008, 6, 6);
            var events = _ncdc.GetEvents(new State("MI"), "Ingham", date, date.AddYears(1), null);
            Assert.AreEqual(1, events.Count());
            var e = events.Single();
            Assert.AreEqual(date, e.Date);
            Assert.AreEqual(WeatherIncidentType.HighWind, e.EventType);
            var location = e.Locations.Single();
            Assert.IsNull(location.StreetAddress);
            Assert.AreEqual("North Leslie", location.City);
            Assert.AreEqual("Ingham", location.County);
            Assert.AreEqual(new State("MI"), location.State);
            Assert.AreEqual(42.4888206, location.Latitude);
            Assert.AreEqual(-84.4281308, location.Longitude);
        }
    }
}
