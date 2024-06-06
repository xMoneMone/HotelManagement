using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using System.Security.Claims;

namespace HotelManagementAPI.Util
{
    public class Jwt
    {
        public static int DecodeUser(IEnumerable<Claim> claims)
        {
            int userId = int.Parse(claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
            return userId;
        }

    }
}
