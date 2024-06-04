using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using System.Security.Claims;

namespace HotelManagementAPI.Util
{
    public class JwtDecoder
    {
        public static User GetUser(IEnumerable<Claim> claims)
        {
            int userId = int.Parse(claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
            return UserStore.GetById(userId);
        }
    }
}
