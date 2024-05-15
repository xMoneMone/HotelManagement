using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class Extra
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int HotelId { get; set; }

    public virtual Hotel Hotel { get; set; } = null!;
}
