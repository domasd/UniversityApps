using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT.Models
{
	public class Flight
	{
	    public string FlightCode { get; set; }

        public decimal Cost { get; set; }

	    public string FromAirportCode { get; set; }

        public string ToAirportCode { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }
    }
}