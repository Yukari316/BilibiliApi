using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using BilibiliApi.Dynamic.CardEnum;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Dynamic
{
    public static class NetUtils
    {
        /// <summary>
        /// 获取链接
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static string GetDynamicUrl(long uid, long offset = 0)
        {
            Dictionary<string, long> urlParams = new Dictionary<string, long>
            {
                {"host_uid", uid},
                {"offset_dynamic_id", offset}
            };
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append("https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/space_history?");
            urlBuilder.Append(string.Join("&", urlParams.Select(param => $"{param.Key}={param.Value}")));
            return urlBuilder.ToString();
        }

        /// <summary>
        /// 从服务器获取最新的动态数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="cardType">动态类型</param>
        /// <returns></returns>
        public static JObject GetBiliDynamicJson(long uid,out CardType cardType)
        {
            //响应JSON
            JObject cardJObject;
            try
            {
                JObject dataJObject = JObject.Parse(GetHttpResponse(GetDynamicUrl(uid)));
                string  code        = dataJObject["code"]?.ToString();
                if (code == null || !code.Equals("0"))
                {
                    cardType = (CardType) (-1);
                    return null;
                }
                //检查是否是置顶动态[4]
                cardJObject = (int)dataJObject["data"]?["cards"]?[0]?["extra"]?["is_space_top"] == 0
                    ? JObject.Parse(dataJObject["data"]?["cards"]?[0]?.ToString() ?? string.Empty)
                    : JObject.Parse(dataJObject["data"]?["cards"]?[1]?.ToString() ?? string.Empty);
                cardType = Enum.IsDefined(typeof(CardType), (int) cardJObject["desc"]?["type"])
                    ? (CardType) ((int) cardJObject["desc"]?["type"])
                    : CardType.Unknown;
            }
            catch (Exception e)
            {
                Console.WriteLine($"获取JSON时发生了错误\n{e}");
                cardType = (CardType)(-1);
                throw;
            }
            return cardJObject;
        }

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