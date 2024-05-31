using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class HotelCodeStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<HotelCode> HotelCodes { get; set; } = new List<HotelCode>();
}
