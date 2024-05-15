using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class Room
{
    public int Id { get; set; }

    public string RoomNumber { get; set; } = null!;

    public decimal PricePerNight { get; set; }

    public string? Notes { get; set; }

    public int HotelId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Hotel Hotel { get; set; } = null!;
}
