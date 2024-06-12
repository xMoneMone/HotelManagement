namespace HotelManagementAPI.Models.DTO
{
    public class RoomsDTO
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; } = null!;

        public decimal PricePerNight { get; set; }

        public string? Notes { get; set; }

        public virtual int Capacity { get; set; }
    }
}
