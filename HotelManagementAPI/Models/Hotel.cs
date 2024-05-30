using System;
using System.Collections.Generic;

namespace HotelManagementAPI.Models;

public partial class Hotel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? CurrencyId { get; set; }

    public int? DownPaymentPercentage { get; set; }

    public int OwnerId { get; set; }

    public virtual Currency? Currency { get; set; }

    public virtual ICollection<Extra> Extras { get; set; } = new List<Extra>();

    public virtual ICollection<HotelCode> HotelCodes { get; set; } = new List<HotelCode>();

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
