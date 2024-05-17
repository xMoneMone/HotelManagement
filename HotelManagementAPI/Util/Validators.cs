using HotelManagementAPI.Data;

namespace HotelManagementAPI.Util
{
    public class Validators
    {
        public static int ColorValidator(int colorId)
        {
            int[] colors = UserStore.context.Colors.Select(x => x.Id).ToArray();
            if (!colors.Contains(colorId))
            {
                return colors[0];
            }

            return colorId;
        }
    }
}
