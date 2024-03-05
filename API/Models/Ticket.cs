using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ScheduleId { get; set; }

    public int CabinTypeId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public int PassportCountryId { get; set; }

    public string BookingReference { get; set; } = null!;

    public bool Confirmed { get; set; }

    public virtual ICollection<AmenitiesTicket> AmenitiesTickets { get; set; } = new List<AmenitiesTicket>();

    public virtual CabinType CabinType { get; set; } = null!;

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
