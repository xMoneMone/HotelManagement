namespace HotelManagementAPI.Models.DTO
{
    public class UserEditDTO : DTO
    {
        public int ColorId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

    }
}
