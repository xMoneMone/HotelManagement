using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class RoomsBed
{
    public int RoomId { get; set; }

    public int BedId { get; set; }

    public virtual Bed Bed { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
