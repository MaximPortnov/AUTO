using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    internal class FlightSearch
    {
        public List<DateTime> Dates { get; set; } = new List<DateTime>();
        public List<string> Times { get; set; } = new List<string>();
        public List<int> FlightTimes { get; set; } = new List<int>();
        public string FlightNumbers { get; set; }
        public decimal TotalCost { get; set; }
        public int Transfers { get; set; }
        public List<int> FlightIds { get; set; } = new List<int>();
        public List<int> RoutesIds { get; set; } = new List<int>();
        public List<int> AiroportIds { get; set; } = new List<int>();
    }
}
