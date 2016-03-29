using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IT.Models;

namespace IT.Controllers
{
    public class DefaultController : ApiController
    {
        Flight[] flights = {
            new Flight { FromAirportCode = "VNO", ToAirportCode = "KUN", Arrival = new DateTime(2015,12,31,23,59,59)
                , Departure = new DateTime(2015,12,31,23,30,0), Cost = 20.0M, FlightCode = "VNOKUN15"},
            new Flight { FromAirportCode = "VNO", ToAirportCode = "STD", Arrival = new DateTime(2015,12,20,6,0,0)
                , Departure = new DateTime(2015,12,20,5,0,0), Cost = 20.0M, FlightCode = "VNOSTD15"},
       };

        public IEnumerable<Flight> GetAllFlights()
        {
            return flights;
        }

        public IHttpActionResult GetFlight(string code, string fromAirportCode, string toAirportCode)
        {
            IEnumerable<Flight> filteredFights = from f in flights
                                          select f;

            if (code != "all")
            {
                filteredFights = filteredFights.Where(x => x.FlightCode == code);
            }
            if (fromAirportCode != "all")
            {
                filteredFights = filteredFights.Where(x => x.FromAirportCode == fromAirportCode);
            }
            if (toAirportCode != "all")
            {
                filteredFights = filteredFights.Where(x => x.ToAirportCode == toAirportCode);
            }

            return Ok(filteredFights);
        }
    }
}
