﻿using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

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

        internal static bool IsLiveDynamic(JToken data)
        {
            if (!(data["modules"]?["module_dynamic"]?["major"]?.HasValues ?? false))
            {
                return false;
            }
            return (data["modules"]?["module_dynamic"]?["major"]?["type"]?.ToString() ?? string.Empty) == "MAJOR_TYPE_LIVE_RCMD";
        }
    }
}
