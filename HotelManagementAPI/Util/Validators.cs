using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Util
{
    public class Validators
    {
        public static int ValidateMultipleChoice<TEntity>(DbSet<TEntity> table, int colorId) where TEntity : class
        {
            if (colorId <= 0 || colorId > table.Count())
            {
                return 1;
            }

            return colorId;
        }
    }
}
