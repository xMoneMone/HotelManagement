using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public int ColorId { get; set; }

    public int? AccountTypeId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual AccountType? AccountType { get; set; }

    public virtual Color Color { get; set; } = null!;
}
