using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BilibiliApi
{
    internal class Util
    {
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="UA">UA</param>
        /// <param name="Timeout">超时</param>
        /// <returns></returns>
        internal static string GetHttpResponse(string url, string UA = "Windows", int Timeout = 3000)
        {
            Dictionary<String, String> UAList = new Dictionary<String, String>
            {
                {
                    "Windows",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36"
                },
                {
                    "IOS",
                    "Mozilla/5.0 (iPhone; CPU iPhone OS 8_3 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Version/8.0 Mobile/12F70 Safari/600.1.4"
                },
                {
                    "Andorid",
                    "Mozilla/5.0 (Linux; Android 4.2.1; M040 Build/JOP40D) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.59 Mobile Safari/537.36"
                }
            };
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method      = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent   = UAList[UA];
            request.Timeout     = Timeout;

            HttpWebResponse response         = (HttpWebResponse)request.GetResponse();
            Stream          myResponseStream = response.GetResponseStream();
            StreamReader    myStreamReader   = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string          retString        = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}
