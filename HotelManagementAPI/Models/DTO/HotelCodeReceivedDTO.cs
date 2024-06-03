namespace HotelManagementAPI.Models.DTO
{
    public class HotelCodeReceivedDTO
    {
        public string Code { get; set; } = null!;

        public string HotelName { get; set; } = null!;

        public string OwnerEmail { get; set;} = null!;
    }
}
