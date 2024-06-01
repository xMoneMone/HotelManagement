namespace HotelManagementAPI.Models.DTO
{
    public class CreateHotelDTO
    {
        public string Name { get; set; } = null!;

        public int? CurrencyId { get; set; }

        public int? DownPaymentPercentage { get; set; }

    }
}
