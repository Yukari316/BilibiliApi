using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Dynamic
{
    /// <summary>
    /// 动态类的API
    /// </summary>
    public static class DynamicAPIs
    {
        /// <summary>
        /// 从服务器获取最新的动态数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns>
        /// 动态数据和动态类型
        /// </returns>
        public static async ValueTask<ulong> GetLatestDynamicId(long uid)
        {
            //响应JSON
            try
            {
                HttpResponseMessage response =
                    await Util.PubHttpClient
                              .GetAsync($"https://api.bilibili.com/x/polymer/web-dynamic/v1/feed/space?host_mid={uid}");

                if (response.StatusCode != HttpStatusCode.OK) return 0;

                JToken responseData = JToken.Parse(await response.Content.ReadAsStringAsync());
                if (responseData["code"]?.ToString() != "0")
                {
                    return 0;
                }

                //判断置顶
                return responseData["data"]?["items"]?[0]?["modules"]?["module_tag"]?["text"]?.ToString() == "置顶"
                    ? Convert.ToUInt64(responseData["data"]?["items"]?[1]?["id_str"] ?? 0)
                    : Convert.ToUInt64(responseData["data"]?["items"]?[0]?["id_str"] ?? 0);
            }
            //出现错误时将重构json信息
            catch
            {
                return 0;
            }
        }
    }
}