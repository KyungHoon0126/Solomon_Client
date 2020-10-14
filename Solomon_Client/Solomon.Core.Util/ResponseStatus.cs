using System;
using System.Net;

namespace Solomon.Core.Util
{
    public class ResponseStatus
    {
        public static int OK = ConvertHttpStatusCodeToInt(HttpStatusCode.OK);
        public static int CREATED = ConvertHttpStatusCodeToInt(HttpStatusCode.Created);

        public static int ConvertHttpStatusCodeToInt(HttpStatusCode status)
        {
            return Convert.ToInt32(status);
        }
    }
}
