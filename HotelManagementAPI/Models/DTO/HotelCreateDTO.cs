namespace HotelManagementAPI.Models.DTO
{
    public class HotelCreateDTO : DTO
    {
        public string Name { get; set; } = null!;

        public int CurrencyId { get; set; }

        public int DownPaymentPercentage { get; set; }

    }
}
