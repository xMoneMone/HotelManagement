namespace HotelManagementAPI.Models.DTO
{
    public class HotelDTO : DTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? CurrencyFormat { get; set; }

        public int? DownPaymentPercentage { get; set; }
    }
}
