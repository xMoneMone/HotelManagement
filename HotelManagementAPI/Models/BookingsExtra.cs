using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class BookingsExtra
{
    public int ExtraId { get; set; }

    public int BookingId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Extra Extra { get; set; } = null!;
}
