using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class Color
{
    public int Id { get; set; }

    public string Color1 { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
