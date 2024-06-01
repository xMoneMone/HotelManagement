namespace HotelManagementAPI.Models.DTO
{
    public class CreateUserDTO
    {
        public int ColorId { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? AccountTypeId { get; set; }
    }
}
