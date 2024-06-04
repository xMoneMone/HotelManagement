namespace HotelManagementAPI.Models.DTO
{
    public class RoomDTO : DTO
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; } = null!;

        public decimal PricePerNight { get; set; }

        public string? Notes { get; set; }

        public string CurrencyFormat { get; set; } = null!;
    }
}
