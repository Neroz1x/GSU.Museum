using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;

namespace GSU.Museum.CommonClassLibrary.Extensions
{
    public static class StringExtensions
    {
        public static void ValidateId(this string id)
        {
            if(id.Length != 24)
            {
                throw new Error(Errors.Invalid_input, "Incorrect id length");
            }
        }
    }
}
