namespace HotelManagementAPI.Models.DTO
{
    public class RoomCreateDTO
    {
        public string RoomNumber { get; set; } = null!;

        public decimal PricePerNight { get; set; }

        public string? Notes { get; set; }

        public int HotelId { get; set; }
    }
}
