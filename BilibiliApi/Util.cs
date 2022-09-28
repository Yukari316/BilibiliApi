using System;
using System.Net;
using System.Net.Http;

namespace BilibiliApi
{
    internal static class Util
    {
        internal static readonly HttpClient PubHttpClient = new();

        /// <summary>
        /// 时间戳转DateTime
        /// <param name="timeStamp">时间戳</param>
        /// </summary>
        public static DateTime ToDateTime(this long timeStamp)
        {
            var unixStartTime = new DateTime(1970, 1, 1, 8, 0, 0, 0);
            return unixStartTime.AddSeconds(timeStamp);
        }
    }
}
