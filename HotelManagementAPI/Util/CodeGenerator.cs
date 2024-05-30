using HotelManagementAPI.Data;
using HotelManagementAPI.Models;

namespace HotelManagementAPI.Util
{
    public class CodeGenerator
    {
        public static string GenerateCode()
        {
            Guid code = Guid.NewGuid();
            return code.ToString();
        }
    }
}
