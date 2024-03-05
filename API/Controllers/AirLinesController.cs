using API.Models;
using API2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [Route("")]
    [ApiController]
    public class AirLinesController : ControllerBase
    {
        AirLinesAnomicContext cont = new AirLinesAnomicContext();
        


        [HttpGet("/test")]
        public IActionResult Test(string fromIata, string toIata, DateTime date, bool threeDays)
        {
            
            FlightClass fc = new FlightClass();
            fc.cont = cont;
            var results = new List<FlightClass.FlightSearchResult>();
            fc.FindFlightsRec(fromIata, toIata, date, new List<int>(), results, new FlightClass.FlightSearchResult(), threeDays);
            Console.WriteLine("asd");
            return Ok(results);
        }


        //[HttpGet("/FindFlights")]
        //public IActionResult FindFlights(string fromIata, string toIata, DateTime date)
        //{
        //    var fromAirportId = cont.Airports.FirstOrDefault(a => a.Iatacode == fromIata)?.Id;
        //    var toAirportId = cont.Airports.FirstOrDefault(a => a.Iatacode == toIata)?.Id;
        //    if (!fromAirportId.HasValue || !toAirportId.HasValue) return BadRequest("Invalid IATA code(s).");

        //    var directFlights = cont.Routes
        //        .Where(r => r.DepartureAirportId == fromAirportId && r.ArrivalAirportId == toAirportId)
        //        .Join(cont.Schedules, r => r.Id, s => s.RouteId, (r, s) => new { r, s })
        //        .Where(rs => rs.s.Date == date)
        //        .Select(rs => new FlightSearchResult
        //        {
        //            Dates = new List<DateTime> { rs.s.Date },
        //            Times = new List<string> { rs.s.Time.ToString() },
        //            FlightNumbers = rs.s.FlightNumber,
        //            TotalCost = rs.s.EconomyPrice, // Assuming EconomyPrice as the cost
        //            Transfers = 0,
        //            FlightIds = new List<int> { rs.s.Id },
        //            AiroportIds = new List<int> { fromAirportId.Value, toAirportId.Value }
        //        }).ToList();

        //    var oneStopFlights = (from firstLeg in cont.Routes
        //                          join secondLeg in cont.Routes on firstLeg.ArrivalAirportId equals secondLeg.DepartureAirportId
        //                          where firstLeg.DepartureAirportId == fromAirportId && secondLeg.ArrivalAirportId == toAirportId
        //                          join firstSchedule in cont.Schedules on firstLeg.Id equals firstSchedule.RouteId
        //                          join secondSchedule in cont.Schedules on secondLeg.Id equals secondSchedule.RouteId
        //                          where firstSchedule.Date == date && secondSchedule.Date == date
        //                          select new FlightSearchResult
        //                          {
        //                              Dates = new List<DateTime> { firstSchedule.Date, secondSchedule.Date },
        //                              Times = new List<string> { firstSchedule.Time.ToString(), secondSchedule.Time.ToString() },
        //                              FlightNumbers = $"{firstSchedule.FlightNumber}, {secondSchedule.FlightNumber}",
        //                              TotalCost = firstSchedule.EconomyPrice + secondSchedule.EconomyPrice,
        //                              Transfers = 1,
        //                              FlightIds = new List<int> { firstSchedule.Id, secondSchedule.Id },
        //                              AiroportIds = new List<int> { fromAirportId.Value, firstLeg.ArrivalAirportId, toAirportId.Value }
        //                          }).ToList();

        //    var twoStopsFlights = (from firstLeg in cont.Routes
        //                           join middleLeg in cont.Routes on firstLeg.ArrivalAirportId equals middleLeg.DepartureAirportId
        //                           join finalLeg in cont.Routes on middleLeg.ArrivalAirportId equals finalLeg.DepartureAirportId
        //                           where firstLeg.DepartureAirportId == fromAirportId && finalLeg.ArrivalAirportId == toAirportId
        //                           join firstSchedule in cont.Schedules on firstLeg.Id equals firstSchedule.RouteId
        //                           join middleSchedule in cont.Schedules on middleLeg.Id equals middleSchedule.RouteId
        //                           join finalSchedule in cont.Schedules on finalLeg.Id equals finalSchedule.RouteId
        //                           where firstSchedule.Date == date && middleSchedule.Date == date && finalSchedule.Date == date
        //                           select new FlightSearchResult
        //                           {
        //                               Dates = new List<DateTime> { firstSchedule.Date, middleSchedule.Date, finalSchedule.Date },
        //                               Times = new List<string> { firstSchedule.Time.ToString(), middleSchedule.Time.ToString(), finalSchedule.Time.ToString() },
        //                               FlightNumbers = $"{firstSchedule.FlightNumber}, {middleSchedule.FlightNumber}, {finalSchedule.FlightNumber}",
        //                               TotalCost = firstSchedule.EconomyPrice + middleSchedule.EconomyPrice + finalSchedule.EconomyPrice,
        //                               Transfers = 2,
        //                               FlightIds = new List<int> { firstSchedule.Id, middleSchedule.Id, finalSchedule.Id },
        //                               AiroportIds = new List<int> { fromAirportId.Value, middleLeg.DepartureAirportId, middleLeg.ArrivalAirportId, toAirportId.Value }
        //                           }).ToList();

        //    var allFlights = directFlights.Concat(oneStopFlights).Concat(twoStopsFlights).ToList();
        //    return Ok(allFlights);
        //}


        [HttpGet("CheckUsers", Name = "CheckUsers")]
        public string checkUsers(string login, string password)
        {
            var user = cont.Users.FirstOrDefault(x => x.Email == login && x.Password == password);
            if (user != null)
            {
                if (user.Active == false)
                    return "block";
                else if (user.RoleId == 1)
                    return "admin";
                else if (user.RoleId == 2)
                    return "user";
                else
                    return "error";
            }
            else
                return "false";
        }
        [HttpGet("/GetManageFlight")]
        public IActionResult GetFlights([FromQuery] string? from = null, [FromQuery] string? to = null, [FromQuery] DateTime? outbound = null, [FromQuery] string? flightNumber = null)
        {
            var result = cont.Schedules
                .Join(cont.Routes, // Присоединяем таблицу Routes
                      schedule => schedule.RouteId,
                      route => route.Id,
                      (schedule, route) => new { Schedule = schedule, Route = route })
                .Join(cont.Airports, // Присоединяем таблицу Airports для Departure
                      combined => combined.Route.DepartureAirportId,
                      airport => airport.Id,
                      (combined, departureAirport) => new { combined.Schedule, combined.Route, DepartureAirport = departureAirport })
                .Join(cont.Airports, // Присоединяем таблицу Airports для Arrival
                      combined => combined.Route.ArrivalAirportId,
                      airport => airport.Id,
                      (combined, arrivalAirport) => new { combined.Schedule, combined.Route, combined.DepartureAirport, ArrivalAirport = arrivalAirport })
                .Join(cont.Aircrafts, // Присоединяем таблицу Aircrafts
                      combined => combined.Schedule.AircraftId,
                      aircraft => aircraft.Id,
                      (combined, aircraft) => new
                      {
                          combined.Schedule.Id,
                          combined.Schedule.Confirmed,
                          combined.Schedule.Date,
                          combined.Schedule.Time,
                          From = combined.DepartureAirport.Iatacode,
                          To = combined.ArrivalAirport.Iatacode,
                          combined.Schedule.FlightNumber,
                          Aircraft = aircraft.Name,
                          EconomyPrice = (double)combined.Schedule.EconomyPrice,
                          BusinessPrice = Math.Round((double)combined.Schedule.EconomyPrice * 1.35, 0),
                          FirstClassPrice = Math.Round((double)combined.Schedule.EconomyPrice * 1.35 * 1.3, 0)
                      });
            if (!string.IsNullOrEmpty(from))
            {
                result = result.Where(x => x.From == from);
            }
            if (!string.IsNullOrEmpty(to))
            {
                result = result.Where(x => x.To == to);
            }
            if (outbound.HasValue)
            {
                result = result.Where(x => x.Date == outbound);
            }
            if (!string.IsNullOrEmpty(flightNumber))
            {
                result = result.Where(x => x.FlightNumber == flightNumber);
            }
            return Ok(result);
        }
        [HttpPut("/PutFlight")]
        public IActionResult putFlight(int id, DateTime date, string time, decimal economyPrice, bool confirmed)
        {
            var flight = cont.Schedules.FirstOrDefault(f => f.Id == id);
            if (flight == null)
            {
                return NotFound();
            }
            flight.Date = date;
            TimeSpan temp;
            if(!TimeSpan.TryParseExact(time, @"hh\:mm", null, out temp))
            {
                Console.WriteLine(temp.ToString() + "|\t|" + time);
                return BadRequest("Неверный формат времени, требуется формат HH:mm.");
            }
            flight.Time = temp;
            flight.EconomyPrice = economyPrice;
            flight.Confirmed = confirmed;
            cont.SaveChanges();
            return Ok();
        }
        [HttpPost("/AddFlight")]
        public IActionResult AddFlight(DateTime date, string time, int flightNumber, string from, string to, int aircraftId, decimal economyPrice, bool confirmed)
        {
            // Найти аэропорты по IATACode
            var fromAirport = cont.Airports.FirstOrDefault(a => a.Iatacode == from);
            var toAirport = cont.Airports.FirstOrDefault(a => a.Iatacode == to);

            if (fromAirport == null || toAirport == null)
            {
                // Возвращаем ошибку, если не найден аэропорт отправления или прибытия
                return BadRequest("One of the airports was not found.");
            }

            // Проверяем, существует ли маршрут между аэропортами
            var route = cont.Routes.FirstOrDefault(r => r.DepartureAirportId == fromAirport.Id && r.ArrivalAirportId == toAirport.Id);

            if (route == null)
            {
                return BadRequest();
            }
            var duplicate = cont.Schedules.Any(s => s.Date == date && s.Time == TimeSpan.ParseExact(time, @"hh\:mm", null) && s.FlightNumber == flightNumber.ToString() && s.RouteId == route.Id);
            if (duplicate)
            {
                // Возвращаем статус Conflict, если найден дубликат
                return Conflict("Duplicate schedule found.");
            }
            // Создаем запись в таблице Schedules
            var schedule = new Schedule
            {
                Date = date,
                Time = TimeSpan.ParseExact(time, @"hh\:mm", null), 
                AircraftId = aircraftId,
                RouteId = route.Id,
                EconomyPrice = economyPrice,
                Confirmed = confirmed,
                FlightNumber = flightNumber.ToString() 
            };

            cont.Schedules.Add(schedule);
            cont.SaveChanges(); 

            return Ok(); 
        }

        [HttpPut("/PutFL")]
        public IActionResult PutFl(DateTime date, string time, int flightNumber, string from, string to, int aircraftId, decimal economyPrice, bool confirmed)
        {
            // Преобразование строки времени в TimeSpan
            TimeSpan parsedTime = TimeSpan.ParseExact(time, @"hh\:mm", null);

            // Поиск существующей записи по дате, времени и номеру рейса
            var schedule = cont.Schedules.FirstOrDefault(s => s.Date == date &&
                                                              s.Time == parsedTime &&
                                                              s.FlightNumber == flightNumber.ToString());

            if (schedule == null)
            {
                // Если запись не найдена, возвращаем NotFound
                return NotFound("Schedule not found.");
            }

            // Найти аэропорты по IATACode
            var fromAirport = cont.Airports.FirstOrDefault(a => a.Iatacode == from);
            var toAirport = cont.Airports.FirstOrDefault(a => a.Iatacode == to);

            if (fromAirport == null || toAirport == null)
            {
                // Возвращаем ошибку, если не найден аэропорт отправления или прибытия
                return BadRequest("One of the airports was not found.");
            }

            // Проверяем, существует ли маршрут между аэропортами
            var route = cont.Routes.FirstOrDefault(r => r.DepartureAirportId == fromAirport.Id &&
                                                         r.ArrivalAirportId == toAirport.Id);

            if (route == null)
            {
                // Если маршрут не существует, создаем ошибку
                return BadRequest("Route not found.");
            }

            // Обновление деталей полета
            schedule.AircraftId = aircraftId;
            schedule.RouteId = route.Id;
            schedule.EconomyPrice = economyPrice;
            schedule.Confirmed = confirmed;

            // Сохранение изменений
            cont.SaveChanges();

            return Ok("Schedule updated successfully.");
        }
        
        [HttpGet("/Offices")]
        public List<Office> getOffices() {
            return cont.Offices.ToList();
        }
        [HttpGet("/Airports")]
        public List<Airport> getAirports()
        {
            return cont.Airports.ToList();
        }
        [HttpGet("/CabinTypes")]
        public List<CabinType> getCabinTypes()
        {
            return cont.CabinTypes.ToList();
        }
        [HttpGet("/Country")]
        public List<Country> getCountry()
        {
            return cont.Countries.ToList();
        }
        [HttpPost("/Users")]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            int maxId = cont.Users.OrderByDescending(u => u.Id).Select(u => u.Id).FirstOrDefault();

            User user = new()
            {
                Id = maxId + 1,
                Email = userModel.EmailAddress,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Birthdate = userModel.Birthdate,
                Password = userModel.Password,
                Active = true,
                RoleId = 2,
                Role = cont.Roles.SingleOrDefault(x => x.Id == 2),
                OfficeId = userModel.OfficeId,
                Office = cont.Offices.SingleOrDefault(x => x.Id == userModel.OfficeId)
            };

            cont.Users.Add(user);
            cont.SaveChanges();

            return Created("true", null);
        }

        [HttpGet("/UsersFromOffice")]
        public List<User> GetUsersFromOffice(int officeId)
        {
            if (officeId == -1)
            {
                return cont.Users.ToList();
            }
            else
            {
                return cont.Users.Where(x => x.OfficeId == officeId).ToList();
            }
        }

        [HttpPut("/UserSwitch")]
        public IActionResult SwitchUser(int userId)
        {
            var user = cont.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                user.Active = !user.Active;
                cont.SaveChanges();
                return Ok("Operation completed");
            }
            else
            {
                return NotFound("User not found");
            }
        }

        [HttpPut("/UserEdit")]
        public IActionResult EditUser([FromBody] UserEditModel userModel)
        {
            var userToUpdate = cont.Users.FirstOrDefault(u => u.Id == userModel.Id);

            if (userToUpdate != null)
            {
                userToUpdate.Email = userModel.EmailAddress;
                userToUpdate.FirstName = userModel.FirstName;
                userToUpdate.LastName = userModel.LastName;
                userToUpdate.OfficeId = userModel.OfficeId;
                userToUpdate.RoleId = userModel.RoleId;

                cont.SaveChanges();

                return Ok("User updated successfully");
            }
            else
            {
                return NotFound("User not found");
            }
        }
    }
}
