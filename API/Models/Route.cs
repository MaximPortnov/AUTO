using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Route
{
    public int Id { get; set; }

    public int DepartureAirportId { get; set; }

    public int ArrivalAirportId { get; set; }

    public int Distance { get; set; }

    public int FlightTime { get; set; }

    public virtual Airport ArrivalAirport { get; set; } = null!;

    public virtual Airport DepartureAirport { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
