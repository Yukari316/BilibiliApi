using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BilibiliApi.Dynamic.Enums;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Dynamic
{
    public static class DynamicAPIs
    {
        /// <summary>
        /// 获取链接
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal static string GetDynamicUrl(ulong uid, string offset = "0")
        {
            Dictionary<string, string> urlParams = new Dictionary<string, string>
            {
                {"host_uid", uid.ToString()},
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
        /// <param name="index">想要获取动态的位置(最大值11),默认为0(最新动态)</param>
        /// <param name="pageOffset">动态页offset</param>
        /// <returns></returns>
        public static JObject GetBiliDynamicJson(ulong uid,out CardType cardType,uint index = 0,string pageOffset = "0")
        {
            if (string.IsNullOrEmpty(pageOffset)) throw new NullReferenceException("pageOffset is null");
            if (index > 11) throw new ArgumentOutOfRangeException(nameof(index));
            //响应JSON
            JObject cardJObject;
            try
            {
                JObject dataJObject = JObject.Parse(Util.GetHttpResponse(GetDynamicUrl(uid, pageOffset)));
                string  code        = dataJObject["code"]?.ToString();
                if (code == null || !code.Equals("0"))
                {
                    cardType = CardType.Error;
                    cardJObject = new JObject
                    {
                        new
                        {
                            code = Convert.ToInt32(code) 
                        }
                    };
                    return cardJObject;
                }
                if (index == 0)
                {
                    cardJObject = (int)dataJObject["data"]?["cards"]?[0]?["extra"]?["is_space_top"] == 0
                        ? JObject.Parse(dataJObject["data"]?["cards"]?[0]?.ToString() ?? string.Empty)
                        : JObject.Parse(dataJObject["data"]?["cards"]?[1]?.ToString() ?? string.Empty);
                }
                else
                {
                    cardJObject = JObject.Parse(dataJObject["data"]?["cards"]?[index]?.ToString() ?? string.Empty);
                }
                cardType = Enum.IsDefined(typeof(CardType), (int) (cardJObject["desc"]?["type"] ?? 0))
                    ? (CardType) ((int) cardJObject["desc"]?["type"])
                    : CardType.Unknown;
                cardJObject["next_page"] = dataJObject["data"]?["next_offset"];
            }
            //出现错误时将重构json信息
            catch (Exception e)
            {
                cardType = CardType.Error;
                cardJObject = new JObject
                {
                    new
                    {
                        code = -1,
                        message = e.Message
                    }
                };
            }
            return cardJObject;
        }
    }
}