using System;
using System.Collections.Generic;

namespace API.Models;

public partial class AmenitiesTicket
{
    public int AmenityId { get; set; }

    public int TicketId { get; set; }

    public decimal Price { get; set; }

    public virtual Amenity Amenity { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
