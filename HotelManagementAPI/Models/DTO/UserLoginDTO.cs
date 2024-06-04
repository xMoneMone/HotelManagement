namespace HotelManagementAPI.Models.DTO
{
    public class UserLoginDTO : DTO
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
