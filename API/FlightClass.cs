using API.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace API
{
    public class FlightClass
    {
        public AirLinesAnomicContext cont { get; set; }
        public class FlightSearchResult
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
            //public List<int> TotalSeats { get; set; } = new List<int>();
            //public List<int> EconomySeats { get; set; } = new List<int>();
            //public List<int> BusinessSeats { get; set; } = new List<int>();
            //public List<int> FirstClassSeats { get; set; } = new List<int>();
        }
        public class FlightSearchRes
        {
            public int ScheduleId { get; set; }
            public DateTime Date { get; set; }
            public TimeSpan Time { get; set; }
            public int FlightTime { get; set; }
            public string? FlightNumber { get; set; }
            public decimal TotalCost { get; set; }
            public int RoutesId {  get; set; }
            public int DepartureAirportId { get; set; }
            public int ArrivalAirportId { get; set; }
            public string DepartureIATACode { get; set; }
            public string ArrivalIATACode { get; set; }
            //public int TotalSeats { get; set; }
            //public int EconomySeats { get; set; }
            //public int BusinessSeats { get; set; }
            //public int FirstClassSeats { get; set; }
        }

        public List<FlightSearchRes> SearchFlights(string from, DateTime date, List<int> excludedAirportIds, bool threeDays)
        {
            //SELECT 
            //    S.ID,
            //    S.Date, 
            //    S.Time, 
            //    S.FlightNumber AS FlightNumbers, 
            //    S.EconomyPrice AS TotalCost, 
            //    R.DepartureAirportID, 
            //    R.ArrivalAirportID, 
            //	Da.IATACode,
            //	AA.IATACode,
            //    R.FlightTime
            //FROM Schedules S
            //JOIN Routes R ON S.RouteID = R.ID
            //JOIN Airports DA ON R.DepartureAirportID = DA.ID
            //JOIN Airports AA ON R.ArrivalAirportID = AA.ID
            //WHERE DA.IATACode = 'ADE'
            //    AND(S.Date > '2017-12-01' OR(S.Date = '2017-12-01' AND S.Time > '04:10:00'))
            //    AND R.ArrivalAirportID NOT IN(SELECT ID FROM Airports WHERE IATACode IN('AUH'));
            TimeSpan max;
            if (threeDays)
            {
                date -= TimeSpan.FromDays(3);
                max = new TimeSpan(6, 0, 0, 0);
            }
            else
            {
                max = new TimeSpan(1, 0, 0, 0);
            }
            TimeSpan maxDifference = new TimeSpan(5, 0, 0);
            var result = cont.Schedules
                .Where(s => (
                            s.Date > date 
                            || (s.Date == date && s.Time > date.TimeOfDay))
                      )
                .Join(cont.Routes,
                      schedule => schedule.RouteId,
                      route => route.Id,
                      (schedule, route) => new { Schedule = schedule, Route = route })
                .Join(cont.Airports,
                      combined => combined.Route.DepartureAirportId,
                      airport => airport.Id,
                      (combined, departureAirport) => new { combined.Schedule, combined.Route, DepartureAirport = departureAirport })
                .Where(combined => combined.DepartureAirport.Iatacode == from)
                .Join(cont.Airports,
                      combined => combined.Route.ArrivalAirportId,
                      airport => airport.Id,
                      (combined, arrivalAirport) => new { combined.Schedule, combined.Route, combined.DepartureAirport, ArrivalAirport = arrivalAirport })
                .Where(combined => !excludedAirportIds.Contains(combined.ArrivalAirport.Id))
                //.Join(cont.Aircrafts, // Joining with Aircrafts to get the TotalSeats
                //      combined => combined.Schedule.AircraftId,
                //      aircraft => aircraft.Id,
                //      (combined, aircraft) => new { combined.Schedule, combined.Route, combined.DepartureAirport, combined.ArrivalAirport, aircraft.TotalSeats, aircraft.EconomySeats, aircraft.BusinessSeats })
                .Select(combined => new FlightSearchRes
                {
                    ScheduleId = combined.Schedule.Id,
                    Date = combined.Schedule.Date,
                    Time = combined.Schedule.Time,
                    FlightNumber = combined.Schedule.FlightNumber,
                    TotalCost = combined.Schedule.EconomyPrice, // Assume EconomyPrice is the TotalCost
                    RoutesId = combined.Schedule.Route.Id,
                    DepartureAirportId = combined.DepartureAirport.Id,
                    ArrivalAirportId = combined.ArrivalAirport.Id,
                    DepartureIATACode = combined.DepartureAirport.Iatacode,
                    ArrivalIATACode = combined.ArrivalAirport.Iatacode,
                    FlightTime = combined.Route.FlightTime,
                    //TotalSeats = combined.TotalSeats,
                    //EconomySeats = combined.EconomySeats,
                    //BusinessSeats = combined.BusinessSeats,
                    //FirstClassSeats = combined.TotalSeats - combined.EconomySeats - combined.BusinessSeats
                }).ToList();

            var filteredFlights = new List<FlightSearchRes>();
            for (int i = 0; i < result.Count - 1; i++)
            {
                var currentFlight = result[i];
                var t1 = currentFlight.Date.Add(currentFlight.Time.Add(TimeSpan.FromMinutes(currentFlight.FlightTime)));
                var t2 = date;
                //Console.WriteLine("-----");
                //Console.WriteLine(t1);
                //Console.WriteLine(t2);
                //Console.WriteLine(t1 - t2);
                
                if (t1-t2 < max)
                {
                    filteredFlights.Add(currentFlight);
                }
            }
            return filteredFlights;
        }


        public void FindFlightsRec(string currentIata, string targetIata, DateTime date, List<int> visitedAirports, List<FlightSearchResult> results, FlightSearchResult currentPath, bool threeDays)
        {
            var flightsFromCurrent = SearchFlights(currentIata, date, visitedAirports, threeDays);
            foreach (var flight in flightsFromCurrent)
            {
                
                var newPath = new FlightSearchResult
                {
                    Dates = new List<DateTime>(currentPath.Dates) { flight.Date },
                    Times = new List<string>(currentPath.Times) { flight.Time.ToString() },
                    FlightTimes = new List<int>(currentPath.FlightTimes) { flight.FlightTime},
                    FlightNumbers = currentPath.FlightNumbers + (string.IsNullOrEmpty(currentPath.FlightNumbers) ? "" : ", ") + flight.FlightNumber,
                    TotalCost = currentPath.TotalCost + flight.TotalCost,
                    Transfers = currentPath.Transfers + 1,
                    FlightIds = new List<int>(currentPath.FlightIds) { flight.ScheduleId },
                    RoutesIds = new List<int>(currentPath.RoutesIds) { flight.RoutesId },
                    AiroportIds = new List<int>(currentPath.AiroportIds) { flight.DepartureAirportId, flight.ArrivalAirportId },
                    //TotalSeats      = new List<int>(currentPath.TotalSeats) { flight.TotalSeats },
                    //EconomySeats    = new List<int>(currentPath.EconomySeats) { flight.EconomySeats },
                    //BusinessSeats   = new List<int>(currentPath.BusinessSeats) { flight.BusinessSeats },
                    //FirstClassSeats = new List<int>(currentPath.FirstClassSeats) { flight.FirstClassSeats }
                };
                if (flight.ArrivalIATACode == targetIata)
                {
                    // Если достигли конечного пункта, добавляем путь в результаты
                    results.Add(newPath);
                }
                else if (!visitedAirports.Contains(flight.ArrivalAirportId))
                {
                    // Если не достигли и есть куда идти дальше, продолжаем поиск рекурсивно
                    var newVisited = new List<int>(visitedAirports) { flight.DepartureAirportId };
                    FindFlightsRec(flight.ArrivalIATACode, targetIata, flight.Date.Add(flight.Time), newVisited, results, newPath, false);
                }
            }
        }
    }
}
