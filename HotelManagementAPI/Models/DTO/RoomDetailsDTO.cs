namespace HotelManagementAPI.Models.DTO
{
    public class RoomDetailsDTO
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; } = null!;

        public decimal PricePerNight { get; set; }

        public string? Notes { get; set; }

        public virtual ICollection<BedDTO> Beds { get; set; } = new List<BedDTO>();
    }
}
