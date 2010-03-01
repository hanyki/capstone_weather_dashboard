﻿using System;
using System.Web.Mvc;
using InsurancePolicyRepository;
using WeatherStation;
using WeatherStation.Geocode;

namespace CapstoneWeatherDashboard.Controllers
{
    public class WeatherIncidentController : Controller
    {
        private readonly IPolicyProvider _insurancePolicyProvider = new MockPolicyProvider();

        public ActionResult Index()
        {
            Address address = null;

            if (!string.IsNullOrEmpty(Request.QueryString["addressSearch"]))
            {
                address = new Address(Request.QueryString["address"], Request.QueryString["city"],
                    Request.QueryString["state"], Request.QueryString["zipCode"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["geocodeSearch"]))
            {
                double latitude = double.Parse(Request.QueryString["latitude"]);
                double longitude = double.Parse(Request.QueryString["longitude"]);

                GoogleGeocodeResponse response = new GoogleGeocoder().ReverseGeocode(latitude, longitude);

                address = new Address(response.Address, response.City, response.State, response.ZipCode);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["policySearch"]))
            {
                string policyNumber = Request.QueryString["policyNumber"];
                string policyHolderName = Request.QueryString["policyHolderName"];

                string policyStreetAddress = Request.QueryString["PolicyStreetAddress"];
                string policyCity = Request.QueryString["PolicyCity"];
                string policyState = Request.QueryString["PolicyState"];
                string policyZipCode = Request.QueryString["PolicyZipCode"];

                if (CanCreateAddressFromData(policyCity, policyState, policyZipCode))
                {
                    address = new Address(policyStreetAddress, policyCity, policyState, policyZipCode);
                }
                else
                {
                    PolicyInfo info =  _insurancePolicyProvider.GetPolicyThatMatchesNameOrNumber(policyNumber, policyHolderName);
                    address = info.PolicyHomeAddress;
                }
            }

            if (address == null)
            {
                // TODO: change to some asp.net mvc validation system
                throw new ArgumentException("Invalid Search");
            }

            DateTime startDate = DateTime.Parse(Request.QueryString["startDate"]);
            DateTime endDate = DateTime.Parse(Request.QueryString["endDate"]);

            string closestAirportCode = address.FetchClosestAirportCode();
            string state = address.State.Abbreviation;
            string county = address.County;

            ViewData["homeAddress"] = address.FullAddress;
            ViewData["latitude"] = address.Latitude;
            ViewData["longitude"] = address.Longitude;
            ViewData["airportCode"] = closestAirportCode;
            ViewData["state"] = state;
            ViewData["county"] = county;
            ViewData["startDate"] = startDate;
            ViewData["endDate"] = endDate;
            ViewData["incidentFilter"] = Request.QueryString["incidentTypes"];

            return View();
        }

        private bool CanCreateAddressFromData(string policyCity, string policyState, string policyZipCode)
        {
            return (!string.IsNullOrEmpty(policyCity) && !string.IsNullOrEmpty(policyState))
                        || !string.IsNullOrEmpty(policyZipCode);
        }
    }
}
