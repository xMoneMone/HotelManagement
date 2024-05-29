using HotelManagementAPI.Models;
using System.Security.Claims;

namespace HotelManagementAPI.Util
{
    public class JwtDecoder
    {
        public static User GetUser(IEnumerable<Claim> claims, HotelManagementContext context)
        {
            int userId = int.Parse(claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
            return context.Users.FirstOrDefault(x => x.Id == userId);
        }
    }
}
