namespace HotelManagementAPI.Models.DTO
{
    public class HotelCreateDTO
    {
        public string Name { get; set; } = null!;

        public int CurrencyId { get; set; }

        public int DownPaymentPercentage { get; set; }

    }
}
