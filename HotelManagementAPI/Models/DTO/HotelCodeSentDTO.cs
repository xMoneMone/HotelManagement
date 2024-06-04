namespace HotelManagementAPI.Models.DTO
{
    public class HotelCodeSentDTO : DTO
    {
        public string Code { get; set; } = null!;

        public string HotelName { get; set; } = null!;

        public string UserEmail { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
