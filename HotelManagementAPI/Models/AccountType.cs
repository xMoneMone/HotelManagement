using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class AccountType
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
