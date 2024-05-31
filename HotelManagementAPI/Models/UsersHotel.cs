using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class UsersHotel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int HotelId { get; set; }

    public virtual Hotel Hotel { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
