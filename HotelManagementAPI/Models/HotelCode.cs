using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class HotelCode
{
    public string Code { get; set; } = null!;

    public int? HotelId { get; set; }

    public int? UserId { get; set; }

    public virtual Hotel? Hotel { get; set; }

    public virtual User? User { get; set; }
}
