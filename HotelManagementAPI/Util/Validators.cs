using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Util
{
    public class Validators
    {
        public static int ValidateMultipleChoice<TEntity>(DbSet<TEntity> table, int id) where TEntity : class
        {
            if (id <= 0 || id > table.Count())
            {
                return 1;
            }

            return id;
        }

        public static bool EmployeeWorksAtHotel(int userId, int[] employeesAtHotel)
        {
            return employeesAtHotel.Contains(userId);
        }
    }
}
