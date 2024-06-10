namespace HotelManagementAPI.Models.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        public int ColorId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
    }
}
