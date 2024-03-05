using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Aircraft
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? MakeModel { get; set; }

    public int TotalSeats { get; set; }

    public int EconomySeats { get; set; }

    public int BusinessSeats { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
