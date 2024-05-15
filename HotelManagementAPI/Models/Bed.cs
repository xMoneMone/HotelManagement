using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class Bed
{
    public int Id { get; set; }

    public string BedType { get; set; } = null!;

    public int Capacity { get; set; }
}
