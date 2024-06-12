namespace HotelManagementAPI.Models.DTO
{
    public class BedOptionsDTO
    {
        public int Id { get; set; }

        public string BedType { get; set; } = null!;

        public int Capacity { get; set; }
    }
}
