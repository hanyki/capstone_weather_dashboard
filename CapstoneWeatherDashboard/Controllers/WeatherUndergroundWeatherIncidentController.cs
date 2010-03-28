﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherStation;
using WeatherStation.WeatherEventProviders;

namespace CapstoneWeatherDashboard.Controllers
{
    public class WeatherUndergroundWeatherIncidentController : Controller
    {
        public ActionResult Index()
        {
            string airportCode = Request.QueryString["airportCode"];
            DateTime date = DateTime.Parse(Request.QueryString["date"]);

            double radius;
            if (!double.TryParse(Request.QueryString["radius"], out radius))
            {
                radius = 15;
            }

            var mainAirport = AirportList.GetAirport(airportCode);
            var airports = new List<Airport> { mainAirport };
            airports.AddRange(AirportList.FindNearbyAirports(mainAirport.Geocode, radius));
            airports = airports.Distinct().ToList();

            var weatherUnderground = new WeatherUnderground();
            List<WeatherIncident> weatherUndergroundIncidents = new List<WeatherIncident>();
            foreach(var airport in airports)
            {
                weatherUndergroundIncidents.AddRange(weatherUnderground.GetEvents(airport.AirportCode, date));
            }
            return Json(weatherUndergroundIncidents.Distinct(), JsonRequestBehavior.AllowGet);
        }

    }
}
