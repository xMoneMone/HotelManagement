﻿using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class Booking
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool DownPaymentPaid { get; set; }

    public bool FullPaymentPaid { get; set; }

    public decimal DownPaymentPrice { get; set; }

    public decimal FullPaymentPrice { get; set; }

    public string? Notes { get; set; }

    public int RoomId { get; set; }

    public virtual Room Room { get; set; } = null!;
}
