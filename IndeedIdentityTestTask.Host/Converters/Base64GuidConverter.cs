using System;

namespace IndeedIdentityTestTask.Host.Converters
{
    public static class Base64GuidConverter
    {
        public static string GuidToBase64(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray()).Replace("/", "-").Replace("+", "_").Replace("=", "");
        }

        public static Guid? Base64ToGuid(this string base64)
        {
            Guid guid;
            base64 = base64.Replace("-", "/").Replace("_", "+") + "==";

            try
            {
                guid = new Guid(Convert.FromBase64String(base64));
            }
            catch (Exception)
            {
                return null;
            }

            return guid;
        }
    }
}
