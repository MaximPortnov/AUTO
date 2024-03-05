using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Amenity
{
    public int Id { get; set; }

    public string Service { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<AmenitiesTicket> AmenitiesTickets { get; set; } = new List<AmenitiesTicket>();

    public virtual ICollection<CabinType> CabinTypes { get; set; } = new List<CabinType>();
}
