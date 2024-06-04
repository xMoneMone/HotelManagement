namespace HotelManagementAPI.Models.DTO
{
    public class BookingCreateDTO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool DownPaymentPaid { get; set; }

        public bool FullPaymentPaid { get; set; }

        public decimal DownPaymentPrice { get; set; }

        public decimal FullPaymentPrice { get; set; }

        public string? Notes { get; set; }
    }
}
