using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    public class Flight
    {
        public int id { get; set; }
        public bool Confirmed { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public String FlightNumber { get; set; }
        public string Aircraft { get; set; }
        public decimal EconomyPrice { get; set; }
        public decimal BusinessPrice { get; set; }
        public decimal FirstClassPrice { get; set; }
    }
}
